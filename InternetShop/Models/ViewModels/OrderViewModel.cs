using InternetShop.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetShop.Models.ViewModels
{
	public class OrderViewModel
	{
		public IEnumerable<Order> Orders { get; set; }
		public decimal TotalPrice { get; set; }
		public Statuses Status { get; set; }
	}
}