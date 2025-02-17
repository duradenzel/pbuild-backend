using Microsoft.EntityFrameworkCore;
using pbuild_domain.Entities;

namespace pbuild_data.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } 

    }
}