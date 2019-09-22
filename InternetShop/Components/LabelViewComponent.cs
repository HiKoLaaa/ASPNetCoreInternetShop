using Microsoft.AspNetCore.Mvc;

namespace InternetShop.Components
{
	public class LabelViewComponent : ViewComponent
	{
		public IViewComponentResult Invoke(string label) => View((object)label);
	}
}