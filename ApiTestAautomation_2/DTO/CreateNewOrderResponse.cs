namespace ApiTestAautomation_2.DTO
{
    public class CreateNewOrderResponse
    {
        public string id { get; set; }
        public string clientId { get; set; }
        public string created { get; set; }
        public string customerName { get; set; }
        public ProductsList[] products { get; set; }
        
        // Add properties for error handling
        public bool HasError { get; set; }
        public String error { get; set; }
    }

    public class ProductsList
    {
        public int id { get; set; }
        public int quantity { get; set; }
    }

    public class ErrorResponse
    {
        public string Message { get; set; }
        // Add more properties if needed
    }
}