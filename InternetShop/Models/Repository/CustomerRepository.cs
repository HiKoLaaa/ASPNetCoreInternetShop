using InternetShop.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InternetShop.Models.Repository
{
	public class CustomerRepository : IRepository<Customer>
	{
		private ProductDbContext _context;

		public CustomerRepository(ProductDbContext productDbContext)
		{
			_context = productDbContext;
		}

		public void AddItem(Customer item)
		{
			if (item.ID == Guid.Empty)
			{
				item.ID = Guid.NewGuid();
			}

			_context.Customers.Add(item);
		}

		public void DeleteItem(Customer item) => _context.Customers.Remove(item);

		public IEnumerable<Customer> GetAllItems() => _context.Customers;

		public Customer GetItem(Guid id) => _context.Customers.Where(c => c.ID == id).FirstOrDefault();

		public void UpdateItem(Customer item)
		{
			Customer newCust = _context.Customers.Where(c => c.ID == item.ID).FirstOrDefault();
			newCust.Name = item.Name;
			newCust.Orders = item.Orders;
			newCust.Address = item.Address;
			newCust.Code = item.Code;
			newCust.Discount = item.Discount;
			_context.Customers.Update(newCust);
		}
	}
}