using InternetShop.Models.DbModels;

namespace InternetShop.Models.ViewModels
{
	public class CartLine
	{
		public Product Product { get; set; }
		public int Quantity { get; set; }
	}
}