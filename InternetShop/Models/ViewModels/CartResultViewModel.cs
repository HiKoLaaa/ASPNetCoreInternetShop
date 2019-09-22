using System.Collections.Generic;

namespace InternetShop.Models.ViewModels
{
	public class CartResultViewModel
	{
		public IEnumerable<CartLine> CartLines { get; set; }
		public decimal TotalPrice { get; set; }
		public string ReturnUrl { get; set; }
	}
}