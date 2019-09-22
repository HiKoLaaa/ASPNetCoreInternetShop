using InternetShop.Models.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace InternetShop.Controllers
{
	public class ShopController : Controller
	{
		private IUnitOfWork _unitOfWork;

		public ShopController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index() => View(_unitOfWork.Products.GetAllItems());
	}
}