using InternetShop.Models.DbModels;
using InternetShop.Models.UnitOfWork;
using InternetShop.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetShop.Controllers
{
	public class OrderController : Controller
	{
		private IUnitOfWork _unitOfWork;
		private UserManager<IdentityUser> _userManager;

		public OrderController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
		{
			_unitOfWork = unitOfWork;
			_userManager = userManager;
		}

		public IActionResult Index(OrderViewModel orderVM)
		{
			return View(new OrderViewModel()
			{
				Orders = _unitOfWork.Orders.GetAllItems()
					.Where(o => o.CustomerID == Guid.Parse(_userManager.GetUserId(User)) &&
						(orderVM.Status == 0 ? true : o.StatusID == (int)orderVM.Status))
			});
		}

		[HttpPost]
		public IActionResult Delete(Guid id, Statuses status)
		{
			Order order = _unitOfWork.Orders.GetItem(id);
			if (order.StatusID != (int)Statuses.New)
			{
				ModelState.AddModelError("", $"Заказ №{order.OrderNumber} уже выполняется/выполнен, удаление невозможно");
				return View(nameof(Index), new OrderViewModel()
				{
					Orders = _unitOfWork.Orders.GetAllItems()
						.Where(o => o.CustomerID == Guid.Parse(_userManager.GetUserId(User)) &&
							(status == 0 ? true : o.StatusID == (int)status))
				});
			}
			else
			{
				_unitOfWork.Orders.DeleteItem(order);
				_unitOfWork.SaveChanges();
			}

			return RedirectToAction(nameof(Index));
		}
	}
}