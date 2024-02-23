using System.Net;
using ApiTestAautomation_2.DTO;
using ApiTestAautomation_2.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Allure.Core;
using RestSharp;

namespace ApiTestAautomation_2;


[AllureNUnit]
public class Tests
{
    RequestsClass request = new RequestsClass();

    private string token;
    private string randomEmail;
    
    [SetUp]
    public void Setup()
    {
        randomEmail = Faker.Internet.Email();
        RestResponse responseWithToken = request.CreateNewUser(randomEmail);
        var jsonResponse = JObject.Parse(responseWithToken.Content);
        token = jsonResponse["token"].ToString();
        
    }
    
    [Test]
    public void PlaceNewOrder()
    {
        
        var response = request.AddOrder(token, randomEmail, 1002, 2);
        var responseContent = JsonConvert.DeserializeObject<CreateNewOrderResponse>(response.Content);
        
        Assert.That(responseContent, Is.Not.Null, "Response should not be null");

        Assert.That(responseContent.clientId, Is.Not.Null.Or.Empty, "clientId should not be null or empty");
        Assert.That(responseContent.id, Is.Not.Null.Or.Empty, "id should not be null or empty");


        Assert.That(responseContent.customerName, Is.Not.Null.Or.Empty, "CustomerName should not be null or empty");
        Assert.That(responseContent.products, Is.InstanceOf<ProductsList[]>(), "Products should be an array of ProductsList");

        // Additional assertions for each item in the array
        foreach (var product in responseContent.products)
        {
            Assert.That(product.id, Is.GreaterThan(0), "Product ID should be greater than 0");
            Assert.That(product.quantity, Is.GreaterThanOrEqualTo(0), "Product quantity should be greater than or equal to 0");
        }
        
        // Console.Write(resp.StatusCode);
        // Console.WriteLine(responseContent.customerName);
        // Console.WriteLine(responseContent.clientId);
        // Console.WriteLine(responseContent.products[0].id);
        // Console.WriteLine(responseContent.products.Length);
        // Console.WriteLine(JObject.Parse(resp.Content));
    }

    [Test]
    public void NewOrderMissingCustomerName()
    {
        var response = request.AddOrder(token, "", 1002, 2);
        var responseContent = JsonConvert.DeserializeObject<CreateNewOrderResponse>(response.Content);
        
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest), "Incorrect Status Code");
        Assert.That(responseContent.error, Is.EqualTo("Customer name is required"));
    }
    
    
    [TestCase(1001)]
    [TestCase(4000)]
    [TestCase(-1001)]
    [TestCase(0)]
    public void NewOrderInvalidProductId(int orderNumber)
    {
        var response = request.AddOrder(token, randomEmail, 9005, 1);
        var responseContent = JsonConvert.DeserializeObject<CreateNewOrderResponse>(response.Content);
        
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest), "Incorrect Status Code");
        Assert.That(responseContent.error, Is.EqualTo("Invalid, unavailable, or zero-quantity products found"));
    }
    
    [TestCase(-1)]
    [TestCase(0)]
    public void NewOrderInvalidQuantity(int quantity)
    {
        var response = request.AddOrder(token, randomEmail, 1001, quantity);
        var responseContent = JsonConvert.DeserializeObject<CreateNewOrderResponse>(response.Content);
        
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest), "Incorrect Status Code.");
        Assert.That(responseContent.error, Is.EqualTo("Invalid, unavailable, or zero-quantity products found"));
    }
    
    
    [Test]
    public void GenericDeserialization()
    {
        // var response = request.AddOrder(token, randomEmail, 1001, 3);
        var client = new RestClient("https://valentinos-coffee.herokuapp.com");
        var restRequest = new RestRequest("/orders", Method.Post);
        var body = new CreateNewOrderRequest
        {
            customerName = randomEmail, products =
            [
                new Products { id = 1001, quantity = 3 }
            ]
        };
        restRequest.AddHeader("x-api-Key", token);
        restRequest.AddJsonBody(body);
        
        var resrResponse = client.Execute(restRequest);
        var responseContent = JsonConvert.DeserializeObject<CreateNewOrderResponse>(resrResponse.Content);
        
        
        Assert.That(resrResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created), "Incorrect Status Code.");
        Assert.That(responseContent.clientId, Is.Not.Null.Or.Empty, "clientId should not be null or empty");
        Assert.That(responseContent.id, Is.Not.Null.Or.Empty, "id should not be null or empty");

    }

    [Test]
    public void GetAllOrders()
    {
        request.AddOrder(token, randomEmail, 1002, 2);
        request.AddOrder(token, randomEmail, 1003, 4);
        var response = request.GetAllOrders(token);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Nothing");
        // List<MyResponseObject> responseObjectList = JsonConvert.DeserializeObject<List<GetAllOrdersResponse>>(response.Content);
        // GetAllOrdersResponse? responseContent = JsonConvert.DeserializeObject<GetAllOrdersResponse>(response.Content);
        

        
        List<GetAllOrdersResponse> responseList = JsonConvert.DeserializeObject<List<GetAllOrdersResponse>>(response.Content);
        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(responseList!, Has.Count.EqualTo(2), response.Content);
            Assert.That(responseList![0].id, Is.Not.Null);
            Assert.That(responseList![0].created, Is.Not.Null);
            Assert.That(responseList![0].customerName, Is.EqualTo(randomEmail));
        });
        Console.WriteLine(JArray.Parse(response.Content));
    }
}