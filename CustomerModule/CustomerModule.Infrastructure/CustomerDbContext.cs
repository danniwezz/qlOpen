using CustomerModule.Core;
using Microsoft.EntityFrameworkCore;


namespace CustomerModule.Infrastructure;
public class CustomerDbContext : DbContext
{
	public DbSet<Customer> Customer { get; set; }
	public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options) { }
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<Customer>().Property(x => x.Id).ValueGeneratedNever();

		modelBuilder.Entity<Customer>()
		   .OwnsMany(x => x.AssignedServices, c =>
		   {
			   c.HasKey(x => x.Id);
			   c.Property(x => x.CustomerId).ValueGeneratedNever();
			   c.Property(x => x.ServiceId).ValueGeneratedNever();
			   c.WithOwner().HasForeignKey(nameof(AssignedService.CustomerId));
			   c.OwnsMany(x => x.Discounts, d =>
			   {
				   d.HasKey(x => x.Id);
				   d.Property(x => x.Id).ValueGeneratedNever();
				   d.Property(x => x.AssignedServiceId).ValueGeneratedNever();
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

		configurationBuilder
			   .Properties<decimal>()
			   .HavePrecision(18, 4);
		configurationBuilder
			.Properties<decimal?>()
			.HavePrecision(18, 4);
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{

		optionsBuilder.EnableDetailedErrors();
	}
}
