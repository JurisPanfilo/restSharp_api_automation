namespace ApiTestAautomation_2.DTO;

public class CreateNewOrderResponse
{
    public string id { get; set; }
    public string clientId { get; set; }
    public string created { get; set; }
    public string customerName { get; set; }
    public ProductsList[] products { get; set; }
}

public class ProductsList
{
    public int id { get; set; }
    public int quantity { get; set; }
}