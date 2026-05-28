namespace CoffeeProductionApp.Models
{
    public class StorageConditionRecord
    {
        public int Id { get; set; }
        public int CellId { get; set; }
        public DateTime MeasurementDate { get; set; }
        public decimal TemperatureC { get; set; }
        public decimal HumidityPercent { get; set; }

        public string CellCode { get; set; }
    }
}