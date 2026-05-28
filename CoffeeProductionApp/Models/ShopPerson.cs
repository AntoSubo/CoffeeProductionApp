namespace CoffeeProductionApp.Models
{
    public class ShopPerson
    {
        public int Id { get; set; }
        public int ShopId { get; set; }
        public int PersonId { get; set; }
        public string Position { get; set; }
        public DateTime? HireDate { get; set; }

        public string ShopName { get; set; }
        public string PersonFullName { get; set; }
    }
}