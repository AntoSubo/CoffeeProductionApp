namespace CoffeeProductionApp.Models
{
    public class Plantation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string Coordinates { get; set; }
        public decimal Area { get; set; }
        public DateTime? PlantingDate { get; set; }
        public string CoffeeVariety { get; set; }
        public string SoilType { get; set; }
        public string IrrigationSystem { get; set; }
    }
}