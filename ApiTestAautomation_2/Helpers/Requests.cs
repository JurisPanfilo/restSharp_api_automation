using ApiTestAautomation_2.DTO;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace ApiTestAautomation_2.Helpers;

public class RequestsClass
{
    
    public RestResponse CreateNewUser(string randomEmail)
    {
        var client = new RestClient("https://valentinos-coffee.herokuapp.com");
        var restRequest = new RestRequest("/clients", Method.Post);
        var body = new CreateUserRequest { email = randomEmail };
        restRequest.AddJsonBody(body);
        RestResponse restResponse = client.Execute(restRequest);
        
        return restResponse;
    }

    public RestResponse AddOrder(string token, string customerName, int orderNumber, int quantity)
    {
        
        var client = new RestClient("https://valentinos-coffee.herokuapp.com");
        var restRequest = new RestRequest("/orders", Method.Post);
        var body = new CreateNewOrderRequest
        {
            customerName = customerName, products =
            [
                new Products { id = orderNumber, quantity = quantity }
            ]
        };
        restRequest.AddHeader("x-api-Key", token);
        restRequest.AddJsonBody(body);
        
        
        var resrResponse = client.Execute(restRequest);

        return resrResponse;
    }
    
}