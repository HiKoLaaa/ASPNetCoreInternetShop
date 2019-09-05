using System;

namespace InternetShop.Models.Repository
{
	public class OrderProduct
	{
		public Guid OrderID { get; set; }
		public Guid ProductID { get; set; }
		public Order Order { get; set; }
		public Product Product { get; set; }
	}
}