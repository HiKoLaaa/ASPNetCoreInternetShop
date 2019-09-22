using InternetShop.Infrastructure;
using System;

namespace InternetShop.Models.ViewModels
{
	public class ConfirmOrderViewModel
	{
		public Guid OrderID { get; set; }

		[MustBeFutureOrNowDate]
		public DateTime ShipmentDate { get; set; }
	}
}