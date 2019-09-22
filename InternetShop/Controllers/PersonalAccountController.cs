using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternetShop.Controllers
{
	[Authorize]
	public class PersonalAccountController : Controller
	{
		public IActionResult Index() => View();
	}
}