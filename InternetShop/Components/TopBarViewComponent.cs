using InternetShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetShop.Components
{
	public class TopBarViewComponent : ViewComponent
	{
		public IViewComponentResult Invoke()
		{
			return View(new MainInfo()
			{
				UserName = User.Identity.Name
			});
		}
	}
}