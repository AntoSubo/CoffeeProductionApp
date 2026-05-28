namespace CoffeeProductionApp.Models
{
    public class StoreReceipt
    {
        public int Id { get; set; }
        public DateTime ReceiptDate { get; set; }
        public int ShopId { get; set; }
        public int FinishedProductBatchId { get; set; }
        public int? Quantity { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? RetailPrice { get; set; }

        public string ShopName { get; set; }
        public string BatchNumber { get; set; }
        public string ProductName { get; set; }
    }
}