using System;
using System.Threading.Tasks;

namespace InternetShop.Models.Repository
{
	public interface IUnitOfWork
	{
		IRepository<Customer> Customers { get; }
		IRepository<Order> Orders { get; }
		IRepository<Product> Products { get; }

		void SaveChanges();
	}
}