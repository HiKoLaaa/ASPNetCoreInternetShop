using System.Collections.Generic;
using Xunit;
using Moq;
using InternetShop.Models.UnitOfWork;
using InternetShop.Models.DbModels;
using InternetShop.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace InternetShop.Tests.Controllers.Tests
{
	public class ShopControllerTests
	{
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

			var unitOfWorkMock = new Mock<IUnitOfWork>();
			unitOfWorkMock.Setup(m => m.Products.GetAllItems()).Returns(products);
			var target = new ShopController(unitOfWorkMock.Object);
			var resultModel = ((target.Index() as ViewResult).Model as IEnumerable<Product>).ToArray();

			Assert.True(resultModel[0].Name == "Name1");
			Assert.True(resultModel[1].Name == "Name4");
			Assert.True(resultModel[2].Name == "Name8");
			Assert.True(resultModel[3].Name == "Name3");
		}
	}
}