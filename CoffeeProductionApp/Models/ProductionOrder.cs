namespace CoffeeProductionApp.Models
{
    public class ProductionOrder
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? PlannedCompletionDate { get; set; }
        public int GreenBeanBatchId { get; set; }
        public int RoastingProfileId { get; set; }
        public string RoastLevel { get; set; }
        public string TargetFlavor { get; set; }
        public decimal PlannedWeightKg { get; set; }
        public decimal? ActualWeightKg { get; set; }
        public DateTime? CompletionDate { get; set; }

        public string BatchNumber { get; set; }
        public string RoastingProfileName { get; set; }
    }
}