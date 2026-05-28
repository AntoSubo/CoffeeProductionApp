namespace CoffeeProductionApp.Models
{
    public class ContractPerson
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public int PersonId { get; set; }
        public string RoleInContract { get; set; }

        public string ContractNumber { get; set; }
        public string PersonFullName { get; set; }
    }
}