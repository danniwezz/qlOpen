using CustomerModule.Core;
using Microsoft.EntityFrameworkCore;


namespace CustomerModule.Infrastructure;
public class CustomerDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>()
            .OwnsMany(x => x.AssignedServices, c =>
            {
                c.HasKey(x => x.Id);
                c.WithOwner().HasForeignKey(nameof(AssignedService.CustomerId));
                 c.OwnsMany(x => x.Discounts, d =>
                {
                    d.HasKey(x => x.Id);
                    d.WithOwner().HasForeignKey(nameof(Discount.AssignedServiceId));
                });
            });
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
           .Properties<DateOnly>()
           .HaveConversion<DateOnlyConverter>();
        configurationBuilder
           .Properties<DateOnly?>()
           .HaveConversion<DateOnlyConverter>();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        optionsBuilder.EnableDetailedErrors();
    }
}
