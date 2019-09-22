using InternetShop.Models.DbModels;

namespace InternetShop.Models.ViewModels
{
	public class AloneOrderInfoViewModel
	{
		public Order Order { get; set; }
		public decimal TotalPrice { get; set; }
	}
}