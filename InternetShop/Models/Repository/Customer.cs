using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternetShop.Models.Repository
{
	public class Customer
	{
		[Key]
		public Guid ID { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public string Code { get; set; }

		[Required]
		public string Address { get; set; }

		[Required]
		public int Discount { get; set; }

		public List<Order> Orders { get; set; }
	}
}