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
        
        // var client = new RestClient("https://valentinos-coffee.herokuapp.com");
        // var restRequest = new RestRequest("/orders", Method.Post);
        // var body = new CreateNewOrderRequest
        // {
        //     customerName = randomEmail, products =
        //     [
        //         new Products { id = 1001, quantity = 2 }
        //     ]
        // };
        // restRequest.AddHeader("x-api-Key", token);
        // restRequest.AddJsonBody(body);
        //
        //
        // var response = client.Execute(restRequest);


        RestResponse resp = request.AddOrder(token, randomEmail, 1002, 2);
        
        var responseContent = JsonConvert.DeserializeObject<CreateNewOrderResponse>(resp.Content);
        
        Console.Write(resp.StatusCode);
        Console.WriteLine(responseContent.customerName);
        Console.WriteLine(responseContent.clientId);
        Console.WriteLine(responseContent.products[0].id);
        Console.WriteLine(responseContent.products.Length);
        Console.WriteLine(JObject.Parse(resp.Content));
        
        
    }
}