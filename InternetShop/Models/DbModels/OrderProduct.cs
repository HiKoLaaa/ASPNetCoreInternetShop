using System;

namespace InternetShop.Models.DbModels
{
	public class OrderProduct
	{
		public Guid OrderID { get; set; }
		public Guid ProductID { get; set; }
		public Order Order { get; set; }
		public Product Product { get; set; }
	}
}