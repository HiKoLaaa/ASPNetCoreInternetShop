using InternetShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace InternetShop.Components
{
	public class TopBarViewComponent : ViewComponent
	{
		private SessionCart _cart;

		public TopBarViewComponent(SessionCart cart)
		{
			_cart = cart;
		}

		public IViewComponentResult Invoke()
		{
			return View(new MainInfoViewModel()
			{
				UserName = User.Identity.Name,
				ProductCount = _cart.Lines.Sum(line => line.Quantity)
			});
		}
	}
}