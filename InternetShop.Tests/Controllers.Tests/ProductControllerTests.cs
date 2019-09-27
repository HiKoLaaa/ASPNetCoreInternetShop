using System;
using System.Collections.Generic;
using System.Linq;
using InternetShop.Controllers;
using InternetShop.Models.DbModels;
using InternetShop.Models.UnitOfWork;
using InternetShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace InternetShop.Tests.Controllers.Tests
{
	public class ProductControllerTests
	{
		public Mock<IUnitOfWork> UnitOfWorkMock { get; set; }

		public SessionCart Cart { get; set; }

		public ProductControllerTests()
		{
			UnitOfWorkMock = new Mock<IUnitOfWork>();
			Cart = new SessionCart();
		}

		[Fact]
		public void Show_All_Products()
		{
			var products = new Product[]
			{
				new Product() { Name = "Name1" },
				new Product() { Name = "Name4" },
				new Product() { Name = "Name8" },
				new Product() { Name = "Name3" },
			};

			UnitOfWorkMock.Setup(m => m.Products.GetAllItems()).Returns(products);
			var target = new ProductController(UnitOfWorkMock.Object, Cart);
			var resultModel = ((target.Index() as ViewResult)?.Model as IEnumerable<Product>).ToArray();

			Assert.True(resultModel[0].Name == "Name1");
			Assert.True(resultModel[1].Name == "Name4");
			Assert.True(resultModel[2].Name == "Name8");
			Assert.True(resultModel[3].Name == "Name3");
		}

		[Fact]
		public void Edit_Product()
		{
			UnitOfWorkMock.Setup(m => m.Products.GetItem(It.IsAny<Guid>())).Returns(new Product() { Name = "Name" });
			var target = new ProductController(UnitOfWorkMock.Object, Cart);
			var result = (target.Edit(It.IsAny<Guid>()) as ViewResult);

			Assert.Equal("Редактирование продукта", result.ViewData["Action"]);
			Assert.Equal("Name", (result.Model as Product)?.Name);
		}

		[Fact]
		public void Create_Valid_Product()
		{
			UnitOfWorkMock.Setup(m => m.Products.GetAllItems()).Returns(new List<Product>());
			UnitOfWorkMock.Setup(m => m.SaveChanges()).Verifiable();
			UnitOfWorkMock.Setup(m => m.Products.AddItem(It.IsAny<Product>())).Verifiable();

			var target = new ProductController(UnitOfWorkMock.Object, Cart);
			var result = (target.Create(new Product())) as RedirectToActionResult;

			Assert.NotNull(result);
			UnitOfWorkMock.Verify(v => v.SaveChanges());
			UnitOfWorkMock.Verify(v => v.Products.AddItem(It.IsAny<Product>()));
		}

		[Fact]
		public void Update_Product()
		{
			Product testProduct = new Product() { Name = "Name1", ID = Guid.Parse("AD65CE6B-10C5-463E-B0E8-61B08A2BE7A8") };
			UnitOfWorkMock.Setup(m => m.Products.GetAllItems()).Returns(new List<Product>() { testProduct });
			UnitOfWorkMock.Setup(m => m.SaveChanges()).Verifiable();
			UnitOfWorkMock.Setup(m => m.Products.UpdateItem(It.IsAny<Product>())).Verifiable();

			var target = new ProductController(UnitOfWorkMock.Object, Cart);
			var result = (target.Create(testProduct)) as RedirectToActionResult;

			Assert.NotNull(result);
			UnitOfWorkMock.Verify(v => v.SaveChanges());
			UnitOfWorkMock.Verify(v => v.Products.UpdateItem(It.IsAny<Product>()));
		}

		[Fact]
		public void Create_Or_Update_Product_Problems()
		{
			Product updateProduct = new Product();
			var target = new ProductController(UnitOfWorkMock.Object, Cart);
			target.ModelState.AddModelError("", "Test error");
			var result = (target.Create(updateProduct)) as RedirectToActionResult;

			Assert.Null(result);
		}

		[Fact]
		public void Delete_Non_Existence_Product()
		{
			UnitOfWorkMock.Setup(m => m.Products.GetItem(It.IsAny<Guid>())).Returns(value: null);
			UnitOfWorkMock.Setup(m => m.Products.DeleteItem(It.IsAny<Product>())).Verifiable();

			var target = new ProductController(UnitOfWorkMock.Object, Cart);
			target.Delete(It.IsAny<Guid>());

			UnitOfWorkMock.Verify(v => v.Products.DeleteItem(It.IsAny<Product>()), Times.Never);
		}

		[Fact]
		public void Delete_Real_Product()
		{
			Product testProduct = new Product() { ID = Guid.Parse("BFF5192C-14F0-4FD7-BCB9-B1EE32737301") };
			UnitOfWorkMock.Setup(m => m.Products.GetItem(It.IsAny<Guid>())).Returns(testProduct);
			UnitOfWorkMock.Setup(m => m.Products.DeleteItem(It.IsAny<Product>())).Verifiable();

			var target = new ProductController(UnitOfWorkMock.Object, Cart);
			target.Delete(testProduct.ID);

			UnitOfWorkMock.Verify(v => v.Products.DeleteItem(It.IsAny<Product>()), Times.Once);
		}
	}
}