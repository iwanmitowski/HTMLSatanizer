using HTMLSatanizer.Models;
using Microsoft.EntityFrameworkCore;

namespace HTMLSatanizer.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Site> Site { get; set; }
    }
}
