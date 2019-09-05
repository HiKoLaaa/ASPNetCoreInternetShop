using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetShop.Models.Repository
{
	public class ProductRepository : IRepository<Product>
	{
		private ProductDbContext _context;

		public ProductRepository(ProductDbContext productDbContext)
		{
			_context = productDbContext;
		}

		public void AddItem(Product item)
		{
			item.ID = Guid.NewGuid();
			_context.Products.Add(item);
		}

		public void DeleteItem(Product item) => _context.Products.Remove(item);

		public IEnumerable<Product> GetAllItems() => _context.Products;

		public Product GetItem(Guid id) => _context.Products.Where(c => c.ID == id).FirstOrDefault();

		public void UpdateItem(Product item) => _context.Products.Update(item);
	}
}