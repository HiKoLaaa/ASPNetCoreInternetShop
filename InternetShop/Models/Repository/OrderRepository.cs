using InternetShop.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InternetShop.Models.Repository
{
	public class OrderRepository : IRepository<Order>
	{
		private ProductDbContext _context;

		public OrderRepository(ProductDbContext productDbContext)
		{
			_context = productDbContext;
		}

		public void AddItem(Order item)
		{
			item.ID = Guid.NewGuid();
			_context.Orders.Add(item);
		}

		public void DeleteItem(Order item) => _context.Orders.Remove(item);

		public IEnumerable<Order> GetAllItems() => _context.Orders;

		public Order GetItem(Guid id) => _context.Orders.Where(c => c.ID == id).FirstOrDefault();

		public void UpdateItem(Order item) => _context.Orders.Update(item);
	}
}