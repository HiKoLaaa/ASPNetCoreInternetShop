using InternetShop.Models.DbModels;
using InternetShop.Models.Repository;

namespace InternetShop.Models.UnitOfWork
{
	public interface IUnitOfWork
	{
		IRepository<Customer> Customers { get; }
		IRepository<Order> Orders { get; }
		IRepository<Product> Products { get; }

		void SaveChanges();
	}
}