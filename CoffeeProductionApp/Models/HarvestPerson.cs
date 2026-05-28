namespace CoffeeProductionApp.Models
{
    public class HarvestPerson
    {
        public int Id { get; set; }
        public int HarvestId { get; set; }
        public int PersonId { get; set; }
        public decimal CollectedWeightKg { get; set; }

        public string PersonFullName { get; set; }
    }
}