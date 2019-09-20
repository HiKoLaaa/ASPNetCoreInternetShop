using InternetShop.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternetShop.Models.ViewModels
{
	public class ConfirmOrderViewModel
	{
		public Guid OrderID { get; set; }

		[MustBeFutureOrNowDate]
		public DateTime ShipmentDate { get; set; }
	}
}