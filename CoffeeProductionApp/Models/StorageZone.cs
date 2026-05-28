namespace CoffeeProductionApp.Models
{
    public class StorageZone
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; }
        public string ZoneName { get; set; }

        public string WarehouseName { get; set; }
    }
}