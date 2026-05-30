namespace CoffeeProductionApp.Models
{
    public class GreenBeanBatch
    {
        public int Id { get; set; }
        public int? HarvestId { get; set; }
        public string BatchNumber { get; set; }
        public DateTime ReceiptDate { get; set; }
        public string CountryOfOrigin { get; set; }
        public string Variety { get; set; }
        public decimal NetWeightKg { get; set; }
        public decimal HumidityPercent { get; set; }
        public int? CellId { get; set; }
        public int? ShelfLifeMonths { get; set; }
    }
}