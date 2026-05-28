namespace CoffeeProductionApp.Models
{
    public class ProductionOrderPerson
    {
        public int Id { get; set; }
        public int ProductionOrderId { get; set; }
        public int PersonId { get; set; }
        public string RoleInOrder { get; set; }

        public string OrderNumber { get; set; }
        public string PersonFullName { get; set; }
    }
}