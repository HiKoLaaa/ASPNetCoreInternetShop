using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetShop.Models.Repository;

namespace InternetShop.Models.ViewModels
{
	public class CartLine
	{
		public Product Product { get; set; }
		public int Quantity { get; set; }
	}
}