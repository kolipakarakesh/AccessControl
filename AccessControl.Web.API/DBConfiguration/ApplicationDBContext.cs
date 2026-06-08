using AccessControl.Web.API.Models;
using Microsoft.EntityFrameworkCore;


namespace AccessControl.Web.API.DBConfiguration
{
    public class ApplicationDbContext : DbContext
    {
       

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> users { get; set; }
        public DbSet<Roles> Roles { get; set; }
    }
}
