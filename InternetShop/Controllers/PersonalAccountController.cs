using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetShop.Controllers
{
	public class PersonalAccountController : Controller
	{
		public IActionResult Index() => View();
	}
}