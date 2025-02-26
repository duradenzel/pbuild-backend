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
        public DbSet<Pokemon> Pokemons { get; set; } 
        public DbSet<Team> Teams { get; set; }  

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Team>()
                .HasMany(t => t.Pokemons)
                .WithOne(p => p.Team)
                .HasForeignKey(p => p.TeamId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}