namespace ApiTestAautomation_2.DTO;

public class CreateNewOrderRequest
{
    public string customerName { get; set; }
    public Products[] products { get; set; }
}

public class Products
{
    public int id { get; set; }
    public int quantity { get; set; }
}