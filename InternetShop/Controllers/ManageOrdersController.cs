using InternetShop.Models.DbModels;
using InternetShop.Models.UnitOfWork;
using InternetShop.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace InternetShop.Controllers
{
	[Authorize(Roles = "Admin")]
	public class ManageOrdersController : Controller
	{
		private IUnitOfWork _unitOfWork;

		public ManageOrdersController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index() => View(_unitOfWork.Orders.GetAllItems());

		[HttpPost]
		public IActionResult CloseOrder(Guid id)
		{
			Order closingOrder = _unitOfWork.Orders.GetItem(id);
			closingOrder.StatusID = (int)Statuses.Done;
			_unitOfWork.SaveChanges();
			return RedirectToAction(nameof(Index));
		}

		public IActionResult ConfirmOrder(Guid id)
		{
			return View(
				new ConfirmOrderViewModel()
				{
					OrderID = id
				});
		}

		[HttpPost]
		public IActionResult ConfirmOrder(ConfirmOrderViewModel confOrdVM)
		{
			if (!ModelState.IsValid)
			{
				return View(confOrdVM);
			}

			Order closingOrder = _unitOfWork.Orders.GetItem(confOrdVM.OrderID);
			closingOrder.StatusID = (int)Statuses.InProgress;
			closingOrder.ShipmentDate = confOrdVM.ShipmentDate;
			_unitOfWork.SaveChanges();
			return RedirectToAction(nameof(Index));
		}
	}
}