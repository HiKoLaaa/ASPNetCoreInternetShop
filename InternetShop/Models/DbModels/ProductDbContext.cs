using Microsoft.EntityFrameworkCore;

namespace InternetShop.Models.DbModels
{
	public class ProductDbContext : DbContext
	{
		public virtual DbSet<Customer> Customers { get; set; }
		public virtual DbSet<Order> Orders { get; set; }
		public virtual DbSet<Product> Products { get; set; }

		public ProductDbContext(DbContextOptions<ProductDbContext> opt)
			: base(opt)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Order>()
				.HasAlternateKey(o => o.OrderNumber);

			modelBuilder.Entity<OrderProduct>()
				.HasOne(op => op.Product)
				.WithMany(p => p.Orders)
				.HasForeignKey(op => op.ProductID);

			modelBuilder.Entity<OrderProduct>()
				.HasOne(op => op.Order)
				.WithMany(o => o.Products)
				.HasForeignKey(op => op.OrderID);
		}
	}
}