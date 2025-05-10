using Microsoft.EntityFrameworkCore;
using CommunicationCoverageSupport.Models.Entities;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CommunicationCoverageSupport.DAL.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ApplicationAdmin> ApplicationAdmins { get; set; }

    }
}
