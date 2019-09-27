using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using InternetShop.Controllers;
using InternetShop.Models.DbModels;
using InternetShop.Models.UnitOfWork;
using InternetShop.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace InternetShop.Tests.Controllers.Tests
{
	public class CartControllerTests
	{
		public Mock<IUnitOfWork> UnitOfWorkMock { get; set; }
		public Mock<UserManager<IdentityUser>> UserManagerMock { get; set; }

		public CartControllerTests()
		{
			UnitOfWorkMock = new Mock<IUnitOfWork>();
			UserManagerMock = new Mock<UserManager<IdentityUser>>(new Mock<IUserStore<IdentityUser>>().Object,
				null, null, null, null, null, null, null, null);
		}

		[Fact]
		public void Make_InValid_Order()
		{
			var cart = new SessionCart();
			var target = new CartController(UnitOfWorkMock.Object, cart, UserManagerMock.Object);

			var result = target.MakeOrder("returnUrl") as RedirectResult;

			Assert.Equal("returnUrl", result.Url);
			UnitOfWorkMock.Verify(m => m.SaveChanges(), Times.Never);
		}

		[Fact]
		public void Make_Valid_Order()
		{
			var cart = new SessionCart();
			cart.AddItem(new Product(), 3);

			UnitOfWorkMock.Setup(m => m.Orders.GetAllItems()).Returns(Enumerable.Empty<Order>());
			UnitOfWorkMock.Setup(m => m.Orders.AddItem(It.IsAny<Order>())).Verifiable();
			UnitOfWorkMock.Setup(m => m.SaveChanges()).Verifiable();
			UserManagerMock.Setup(m => m.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(Guid.NewGuid().ToString());
			var target = new CartController(UnitOfWorkMock.Object, cart, UserManagerMock.Object);

			var result = target.MakeOrder("returnUrl") as RedirectResult;

			UnitOfWorkMock.Verify(m => m.SaveChanges());
			UnitOfWorkMock.Verify(m => m.Orders.AddItem(It.IsAny<Order>()));
			Assert.Equal("returnUrl", result.Url);
		}
	}
}