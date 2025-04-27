namespace CommunicationCoverageSupport.Models.Entities
{
    public class Company
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
    }
}
