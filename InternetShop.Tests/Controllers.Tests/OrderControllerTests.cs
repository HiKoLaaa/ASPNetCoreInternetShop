using System;
using Xunit;
using Moq;
using InternetShop.Models.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using InternetShop.Models.DbModels;
using InternetShop.Controllers;
using InternetShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace InternetShop.Tests.Controllers.Tests
{
	public class OrderControllerTests
	{
		public Mock<IUnitOfWork> UnitOfWorkMock { get; set; }
		public Mock<UserManager<IdentityUser>> UserManagerMock { get; set; }

		public OrderControllerTests()
		{
			UnitOfWorkMock = new Mock<IUnitOfWork>();
			var userStorageMock = new Mock<IUserStore<IdentityUser>>();
			UserManagerMock = new Mock<UserManager<IdentityUser>>(userStorageMock.Object,
				null, null, null, null, null, null, null, null);
		}

		[Fact]
		public void Show_Orders_Without_Filters()
		{
			Guid custId = Guid.NewGuid();
			Order[] orders = new Order[]
			{
				new Order() { CustomerID = Guid.NewGuid() },
				new Order() { CustomerID = custId },
				new Order() { CustomerID = custId },
			};

			UnitOfWorkMock.Setup(m => m.Orders.GetAllItems()).Returns(orders);
			UserManagerMock.Setup(m => m.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(custId.ToString());
			var target = new OrderController(UnitOfWorkMock.Object, UserManagerMock.Object);

			var resultModel = (target.Index(new OrderViewModel()) as ViewResult).Model as OrderViewModel;

			Assert.Equal(2, resultModel.Orders.Count());
			Assert.True(resultModel.Orders.First().CustomerID == custId);
		}

		[Fact]
		public void Show_Orders_With_Some_Filter()
		{
			Guid custId = Guid.NewGuid();
			Order[] orders = new Order[]
			{
				new Order() { CustomerID = Guid.NewGuid(), StatusID = 1 },
				new Order() { CustomerID = custId, StatusID = 2 },
				new Order() { CustomerID = custId, StatusID = 3 },
			};

			UnitOfWorkMock.Setup(m => m.Orders.GetAllItems()).Returns(orders);
			UserManagerMock.Setup(m => m.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(custId.ToString());
			var target = new OrderController(UnitOfWorkMock.Object, UserManagerMock.Object);

			var resultModel = (target.Index(new OrderViewModel()
			{
				Status = Statuses.InProgress
			}) as ViewResult).Model as OrderViewModel;

			Assert.Single(resultModel.Orders);
			Assert.True(resultModel.Orders.First().StatusID == 2);
		}

		[Fact]
		public void Valid_Delete_Order()
		{
			UnitOfWorkMock.Setup(m => m.Orders.GetItem(It.IsAny<Guid>())).Returns(new Order() { StatusID = 3 });
			UnitOfWorkMock.Setup(m => m.Orders.DeleteItem(It.IsAny<Order>())).Verifiable();
			var target = new OrderController(UnitOfWorkMock.Object, UserManagerMock.Object);

			var result = target.Delete(It.IsAny<Guid>(), It.IsAny<Statuses>()) as RedirectToActionResult;

			Assert.NotNull(result);
			UnitOfWorkMock.Verify(uow => uow.Orders.DeleteItem(It.IsAny<Order>()));
		}

		[Fact]
		public void Invalid_Delete_Order()
		{
			UnitOfWorkMock.Setup(m => m.Orders.GetItem(It.IsAny<Guid>())).Returns(new Order() { StatusID = 2 });
			var target = new OrderController(UnitOfWorkMock.Object, UserManagerMock.Object);

			var result = target.Delete(It.IsAny<Guid>(), It.IsAny<Statuses>()) as ViewResult;

			Assert.NotNull(result);
			UnitOfWorkMock.Verify(uow => uow.Orders.DeleteItem(It.IsAny<Order>()), Times.Never);
		}

		[Fact]
		public void Show_One_Order_Info()
		{
			var orderMock = new Mock<Order>();
			Order myOrder = orderMock.Object;
			myOrder.OrderNumber = 3;
			Order[] orders = new Order[]
			{
				new Order() { OrderNumber = 1 },
				myOrder,
				new Order() { OrderNumber = 2 }
			};

			myOrder.Products.Add(new OrderProduct() { Product = new Product() { Price = 100 }, ProductCount = 1 });
			myOrder.Products.Add(new OrderProduct() { Product = new Product() { Price = 200 }, ProductCount = 2 });
			UserManagerMock.Setup(usm => usm.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(Guid.NewGuid().ToString());
			UnitOfWorkMock.Setup(unm => unm.Orders.GetAllItems()).Returns(orders);
			UnitOfWorkMock.Setup(unm => unm.Customers.GetItem(It.IsAny<Guid>())).Returns(new Customer() { Discount = 10 });
			var targer = new OrderController(UnitOfWorkMock.Object, UserManagerMock.Object);

			var resultModel = (targer.FullInfo(orderMock.Object.OrderNumber) as ViewResult)
				.Model as AloneOrderInfoViewModel;

			Assert.Same(myOrder, resultModel.Order);
			Assert.Equal(450, resultModel.TotalPrice);
		}
	}
}