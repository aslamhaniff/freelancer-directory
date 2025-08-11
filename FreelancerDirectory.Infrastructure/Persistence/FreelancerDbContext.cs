using Microsoft.EntityFrameworkCore;
using FreelancerDirectory.Domain.Entities;

namespace FreelancerDirectory.Infrastructure.Persistence;

public class FreelancerDbContext : DbContext
{
    public DbSet<Freelancer> Freelancers { get; set; }
    public DbSet<Skillset> Skillsets { get; set; }
    public DbSet<Hobby> Hobbies { get; set; }

    public FreelancerDbContext(DbContextOptions<FreelancerDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Freelancer>()
            .HasMany(f => f.Skillsets)
            .WithOne(s => s.Freelancer)
            .HasForeignKey(s => s.FreelancerId);

        modelBuilder.Entity<Freelancer>()
            .HasMany(f => f.Hobbies)
            .WithOne(h => h.Freelancer)
            .HasForeignKey(h => h.FreelancerId);
    }
}

