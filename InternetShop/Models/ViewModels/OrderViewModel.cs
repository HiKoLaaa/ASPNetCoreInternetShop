using InternetShop.Models.DbModels;
using System.Collections.Generic;

namespace InternetShop.Models.ViewModels
{
	public class OrderViewModel
	{
		public IEnumerable<Order> Orders { get; set; }
		public decimal TotalPrice { get; set; }
		public Statuses Status { get; set; }
	}
}