using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetShop.Models.ViewModels
{
	public class CartResultViewModel
	{
		public IEnumerable<CartLine> CartLines { get; set; }

		public decimal TotalPrice { get; set; }

		public string ReturnUrl { get; set; }
	}
}