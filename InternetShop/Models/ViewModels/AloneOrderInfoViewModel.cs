using InternetShop.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetShop.Models.ViewModels
{
	public class AloneOrderInfoViewModel
	{
		public Order Order { get; set; }

		public decimal TotalPrice { get; set; }
	}
}