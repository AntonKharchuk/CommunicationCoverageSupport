namespace CommunicationCoverageSupport.Models.Entities
{
    public class ApplicationUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
