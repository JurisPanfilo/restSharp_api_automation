using System.Net;
using ApiTestAautomation_2.DTO;
using ApiTestAautomation_2.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace ApiTestAautomation_2;


public class Tests
{
    RequestsClass request = new RequestsClass();

    private String token;
    private String randomEmail;
    
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
        
        RestResponse resp = request.AddOrder(token, "dewitt@olson.info", 1002, 2);
        var responseContent = JsonConvert.DeserializeObject<CreateNewOrderResponse>(resp.Content);
        
        // Assuming you have deserialized the response into a variable named 'responseContent'
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
            // Add more assertions as needed based on your requirements for each product
        }
        
        Console.Write(resp.StatusCode);
        Console.WriteLine(responseContent.customerName);
        Console.WriteLine(responseContent.clientId);
        Console.WriteLine(responseContent.products[0].id);
        Console.WriteLine(responseContent.products.Length);
        Console.WriteLine(JObject.Parse(resp.Content));
    }
}