using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetShop.Models.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private ProductDbContext _context;

		private IRepository<Customer> _customers;
		public IRepository<Customer> Customers
		{
			get
			{
				if (_customers == null)
				{
					_customers = new CustomerRepository(_context);
				}

				return _customers;
			}
		}

		private IRepository<Order> _orders;
		public IRepository<Order> Orders
		{
			get
			{
				if (_orders == null)
				{
					_orders = new OrderRepository(_context);
				}

				return _orders;
			}
		}

		private IRepository<Product> _products;
		public IRepository<Product> Products
		{
			get
			{
				if (_products == null)
				{
					_products = new ProductRepository(_context);
				}

				return _products;
			}
		}

		public UnitOfWork(ProductDbContext productDbContext)
		{
			_context = productDbContext;
		}

		public void SaveChanges()
		{
			_context.SaveChanges();
		}
	}
}