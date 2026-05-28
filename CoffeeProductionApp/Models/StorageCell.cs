namespace CoffeeProductionApp.Models
{
    public class StorageCell
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public string CellCode { get; set; }

        public string ZoneName { get; set; }
        public string WarehouseName { get; set; }
    }
}