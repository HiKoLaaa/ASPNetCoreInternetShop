using System;
using System.ComponentModel.DataAnnotations;

namespace InternetShop.Models.DbModels
{
	public class OrderProduct
	{
		[Key]
		public Guid ID { get; set; }

		public Guid OrderID { get; set; }
		public Guid ProductID { get; set; }

		[Required]
		public int ProductCount { get; set; }

		public Order Order { get; set; }
		public Product Product { get; set; }
	}
}