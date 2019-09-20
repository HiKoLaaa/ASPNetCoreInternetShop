using InternetShop.Models.DbModels;
using InternetShop.Models.UnitOfWork;
using InternetShop.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InternetShop.Controllers
{
	[Authorize]
	public class CartController : Controller
	{
		private IUnitOfWork _unitOfWork;
		private SessionCart _cart;
		private UserManager<IdentityUser> _userManager;

		public CartController(IUnitOfWork unitOfWork, SessionCart cart, UserManager<IdentityUser> userManager)
		{
			_cart = cart;
			_unitOfWork = unitOfWork;
			_userManager = userManager;
		}

		public IActionResult Index()
		{
			decimal priceWithoutDiscont = _cart.ComputeTotalValue();
			Customer currCust = _unitOfWork.Customers.GetItem(Guid.Parse(_userManager.GetUserId(User)));
			decimal priceWithDiscout = priceWithoutDiscont - priceWithoutDiscont * (currCust.Discount / 100);
			return View(new CartResultViewModel()
			{
				CartLines = _cart.Lines,
				TotalPrice = priceWithDiscout
			});
		}

		[HttpPost]
		public IActionResult RemoveFromCart(Guid productID)
		{
			_cart.RemoveLine(_unitOfWork.Products.GetItem(productID));
			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		public IActionResult MakeOrder()
		{
			if (!(_cart.Lines.Count() == 0))
			{
				Order order = new Order()
				{
					CustomerID = Guid.Parse(_userManager.GetUserId(User)),
					OrderDate = DateTime.Now,
					ShipmentDate = null,
					OrderNumber = _unitOfWork.Orders.GetAllItems().Count() + 1,
					ProductCount = _cart.Lines.Sum(p => p.Quantity),
					StatusID = (int)Statuses.New,
					Products = new List<OrderProduct>()
				};

				foreach (var line in _cart.Lines)
				{
					order.Products.Add(new OrderProduct()
					{
						OrderID = order.ID,
						ProductID = line.Product.ID
					});
				}

				_unitOfWork.Orders.AddItem(order);
				_unitOfWork.SaveChanges();
				_cart.Clear();
			}

			return RedirectToAction(nameof(Index));
		}
	}
}