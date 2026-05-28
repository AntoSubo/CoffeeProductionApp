namespace CoffeeProductionApp.Models
{
    public class B2BOrderItem
    {
        public int Id { get; set; }
        public int B2BOrderId { get; set; }
        public int ProductCatalogId { get; set; }
        public int Quantity { get; set; }
        public decimal? WholesalePrice { get; set; }

        public string OrderNumber { get; set; }
        public string ProductName { get; set; }
    }
}