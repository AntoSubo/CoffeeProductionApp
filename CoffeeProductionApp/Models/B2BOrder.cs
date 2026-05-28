namespace CoffeeProductionApp.Models
{
    public class B2BOrder
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public int ContractId { get; set; }
        public DateTime? ShipmentDate { get; set; }
        public string DeliveryMethod { get; set; }
        public string TtnNumber { get; set; }
        public string Status { get; set; }

        public string ContractNumber { get; set; }
        public string LegalEntityName { get; set; }
    }
}