using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetShop.Components
{
	public class LabelViewComponent : ViewComponent
	{
		public IViewComponentResult Invoke(string label) => View((object)label);
	}
}