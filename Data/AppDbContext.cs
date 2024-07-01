using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Person> People { get; set; }
    public DbSet<Club> Club { get; set; }
    public DbSet<Players> Players { get; set; }
    public DbSet<TransferRequest> TransferRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Club>()
            .Property(c => c.Budget)
            .HasPrecision(18, 2); 

        base.OnModelCreating(modelBuilder);

        base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Players>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Club>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<TransferRequest>()
                .HasKey(tr => tr.Id);
    }
 
}
