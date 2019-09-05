using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternetShop.Models.Repository
{
	public class Product
	{
		[Key]
		public Guid ID { get; set; }

		[Required]
		// TODO: [RegularExpression()]
		public string Code { get; set; }

		public string Name { get; set; }
		public decimal Price { get; set; }

		[StringLength(25)]
		public string Category { get; set; }

		public List<OrderProduct> Orders { get; set; }
	}
}
