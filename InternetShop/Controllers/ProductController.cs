using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InternetShop.Models.Repository;

namespace InternetShop.Controllers
{
	[Authorize]
	public class ProductController : Controller
	{
		private IUnitOfWork _unitOfWork;

		public ProductController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index() => View(_unitOfWork.Products.GetAllItems());

		public IActionResult Create()
		{
			ViewBag.Action = "Добавление продукта";
			return View();
		}

		public IActionResult Edit(Guid id)
		{
			ViewBag.Action = "Редактирование продукта";
			return View(nameof(Create), _unitOfWork.Products.GetItem(id));
		}

		[HttpPost]
		public IActionResult Create(Product product)
		{
			if (_unitOfWork.Products.GetAllItems().Where(p => p.ID == product.ID).FirstOrDefault() == null)
			{
				_unitOfWork.Products.AddItem(product);
			}
			else
			{
				_unitOfWork.Products.UpdateItem(product);
			}

			_unitOfWork.SaveChanges();
			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		public IActionResult Delete(Guid id)
		{
			Product p = _unitOfWork.Products.GetItem(id);
			if (p != null)
			{
				_unitOfWork.Products.DeleteItem(p);
			}

			_unitOfWork.SaveChanges();
			return RedirectToAction(nameof(Index));
		}
	}
}