namespace CoffeeProductionApp.Models
{
    public class Harvest
    {
        public int Id { get; set; }
        public int PlantationId { get; set; }
        public DateTime HarvestDate { get; set; }
        public string ProcessingMethod { get; set; }
        public decimal BerriesWeightKg { get; set; }
        public decimal DryingHumidity { get; set; }
        public decimal GreenBeansWeightKg { get; set; }
        public decimal DefectPercentage { get; set; }

        public string PlantationName { get; set; }
    }
}