namespace CoffeeProductionApp.Models
{
    public class ProductCatalog
    {
        public int Id { get; set; }
        public string Sku { get; set; }
        public string TradeName { get; set; }
        public string Variety { get; set; }
        public string CountryOfOrigin { get; set; }
        public int? GrowingAltitude { get; set; }
        public string RoastLevel { get; set; }
        public string FlavorNotes { get; set; }
        public string DefaultPackageType { get; set; }
    }
}