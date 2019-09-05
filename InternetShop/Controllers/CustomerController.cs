using InternetShop.Models.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetShop.Controllers
{
	public class CustomerController : Controller
	{
		private IUnitOfWork _unitOfWork;

		public CustomerController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult AllItems() => View(_unitOfWork.Products.GetAllItems());
	}
}