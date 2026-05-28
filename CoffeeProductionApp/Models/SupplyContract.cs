namespace CoffeeProductionApp.Models
{
    public class SupplyContract
    {
        public int Id { get; set; }
        public string ContractNumber { get; set; }
        public DateTime ConclusionDate { get; set; }
        public string LegalEntityName { get; set; }
        public string Inn { get; set; }
        public string PaymentTerms { get; set; }
        public decimal? MinOrderKg { get; set; }
    }
}