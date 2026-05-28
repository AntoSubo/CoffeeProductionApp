namespace CoffeeProductionApp.Models
{
    public class FinishedProductBatch
    {
        public int Id { get; set; }
        public string BatchNumber { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int ProductionOrderId { get; set; }
        public int ProductCatalogId { get; set; }
        public string PackageType { get; set; }
        public int? PackageCount { get; set; }
        public decimal? UnitCost { get; set; }
        public decimal? RetailPrice { get; set; }
        public decimal? WholesalePrice { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public string OrderNumber { get; set; }
        public string ProductName { get; set; }
    }
}