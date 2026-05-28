namespace CoffeeProductionApp.Models
{
    public class AgroOperation
    {
        public int Id { get; set; }
        public int PlantationId { get; set; }
        public int OperationTypeId { get; set; }
        public DateTime OperationDate { get; set; }
        public string Materials { get; set; }
        public string Remarks { get; set; }

        // Навигационные свойства (для отображения)
        public string PlantationName { get; set; }
        public string OperationTypeName { get; set; }
    }
}