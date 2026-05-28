namespace CoffeeProductionApp.Models
{
    public class Certificate
    {
        public int Id { get; set; }
        public string CertificateNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string CertificationBody { get; set; }
        public int ProductCatalogId { get; set; }
        public string Standard { get; set; }

        public string ProductName { get; set; }
    }
}