using InternetShop.Models.ViewModels;
using InternetShop.Models.DbModels;
using Xunit;
using InternetShop.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace InternetShop.Tests.Components.Tests
{
	public class TopBarViewComponentTests
	{
		[Fact]
		public void Count_Products()
		{
			SessionCart cart = new SessionCart();
			cart.AddItem(new Product(), 3);
			cart.AddItem(new Product(), 6);
			cart.AddItem(new Product(), 17);
			cart.AddItem(new Product(), 14);
			var target = new TopBarViewComponent(cart);

			var resultModel = (target.Invoke() as ViewViewComponentResult).ViewData.Model as MainInfoViewModel;

			Assert.NotNull(resultModel);
			Assert.Equal(40, resultModel.ProductCount);
		}
	}
}