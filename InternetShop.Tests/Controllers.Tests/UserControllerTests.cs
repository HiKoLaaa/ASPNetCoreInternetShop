using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using InternetShop.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using InternetShop.Models.ViewModels;
using InternetShop.Models.UnitOfWork;
using InternetShop.Models.DbModels;

namespace InternetShop.Tests.Controllers.Tests
{
	public class UserControllerTests
	{
		public Mock<IUserStore<IdentityUser>> UserStoreMock { get; set; }
		public Mock<UserManager<IdentityUser>> UserManagerMock { get; set; }
		public Mock<IUnitOfWork> UnitOfWorkMock { get; set; }
		public Mock<IUserValidator<IdentityUser>> UserValidatorMock { get; set; }

		public UserControllerTests()
		{
			UserStoreMock = new Mock<IUserStore<IdentityUser>>();
			UserManagerMock = new Mock<UserManager<IdentityUser>>(UserStoreMock.Object,
				null, null, null, null, null, null, null, null);

			UnitOfWorkMock = new Mock<IUnitOfWork>();
			UserValidatorMock = new Mock<IUserValidator<IdentityUser>>();
		}

		[Fact]
		public void Show_Two_Users()
		{

			UserManagerMock.Setup(m => m.Users)
				.Returns(new IdentityUser[]
				{
					new IdentityUser("User1"),
					new IdentityUser("User2")
				}
				.AsQueryable());

			var target = new UserController(UserManagerMock.Object, null, null, null, null, null);

			var resultModel = (target.AllUsers() as ViewResult).Model as IEnumerable<IdentityUser>;
			var resultArr = resultModel.ToArray();

			Assert.Equal(2, resultArr.Length);
			Assert.True(resultArr[0].UserName == "User1");
			Assert.True(resultArr[1].UserName == "User2");
		}

		[Fact]
		public void Delete_One_User()
		{
			IdentityUser[] users = new IdentityUser[]
			{
				new IdentityUser("User1") { Id = "9E922BD2-4395-4524-8264-4F218D600E9D" },
				new IdentityUser("User2") { Id = "4192883F-AA46-42EA-B1DD-73E8CAA7A7B3" }
			};

			UserManagerMock.Setup(m => m.Users).Returns(users.AsQueryable());
			UserManagerMock.Setup(m => m.DeleteAsync(It.IsAny<IdentityUser>()))
				.ReturnsAsync(IdentityResult.Success);

			var target = new UserController(UserManagerMock.Object, null, null, null, null, null);
			var resultAction = (target.Delete(Guid.Parse(users[0].Id))).Result as RedirectToActionResult;

			Assert.NotNull(resultAction);
		}

		[Fact]
		public void Cannot_Delete_One_User()
		{
			IdentityUser[] users = new IdentityUser[]
			{
				new IdentityUser("User1") { Id = "9E922BD2-4395-4524-8264-4F218D600E9D" },
				new IdentityUser("User2") { Id = "4192883F-AA46-42EA-B1DD-73E8CAA7A7B3" }
			};

			UserManagerMock.Setup(m => m.Users).Returns(users.AsQueryable());
			UserManagerMock.Setup(m => m.DeleteAsync(It.IsAny<IdentityUser>()))
				.ReturnsAsync(IdentityResult.Failed());

			var target = new UserController(UserManagerMock.Object, null, null, null, null, null);
			var resultAction = (target.Delete(Guid.Parse(users[0].Id))).Result as RedirectToActionResult;

			Assert.Null(resultAction);
		}

		[Fact]
		public void Create_User()
		{
			var targer = new UserController(UserManagerMock.Object, null, null, null, null, null);
			var resultAction = targer.Create() as ViewResult;

			Assert.NotNull(resultAction);
			Assert.Equal("Добавление нового пользователя", resultAction.ViewData["Action"]);
		}

		[Fact]
		public async Task Edit_Not_Admin()
		{
			IdentityUser[] users = new IdentityUser[]
			{
				new IdentityUser("User1") { Id = "9E922BD2-4395-4524-8264-4F218D600E9D", Email = "Email1@com.com" },
				new IdentityUser("User2") { Id = "4192883F-AA46-42EA-B1DD-73E8CAA7A7B3", Email = "Email2@com.com" }
			};

			UnitOfWorkMock.Setup(m => m.Customers.GetItem(It.IsAny<Guid>()))
				.Returns(new Customer() { Name = "NameCustomer" });

			UserManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(users[0]);
			UserManagerMock.Setup(m => m.IsInRoleAsync(It.IsAny<IdentityUser>(), "Admin")).ReturnsAsync(false);

			var target = new UserController(UserManagerMock.Object, null, null, null, UnitOfWorkMock.Object, null);
			var resultAction = await target.Edit(Guid.Parse(users[0].Id));
			var resultModel = (resultAction as ViewResult).Model as UserInfoViewModel;

			Assert.True(resultModel.IsAdmin == false);
			Assert.True(resultModel.Customer.Name == "NameCustomer");
			Assert.True(resultModel.Email == "Email1@com.com");
		}

		[Fact]
		public async Task Create_New_User()
		{
			UserManagerMock.SetupSequence(m => m.FindByEmailAsync(It.IsAny<string>()))
				.ReturnsAsync(value: null).ReturnsAsync(new IdentityUser());

			UserManagerMock.Setup(m => m.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
				.ReturnsAsync(IdentityResult.Success);

			UnitOfWorkMock.Setup(um => um.Customers.AddItem(It.IsAny<Customer>())).Verifiable();
			UnitOfWorkMock.Setup(um => um.SaveChanges()).Verifiable();
			var target = new UserController(UserManagerMock.Object, null, null, null, UnitOfWorkMock.Object, null);

			var resultAction = await target.Create(new UserInfoViewModel()
			{
				Customer = new Customer(),
				Email = "Email@com.com",
				Password = "123",
				IsAdmin = false
			}) as RedirectToActionResult;

			Assert.NotNull(resultAction);
			UnitOfWorkMock.Verify(e => e.SaveChanges());
			UnitOfWorkMock.Verify(e => e.Customers.AddItem(It.IsAny<Customer>()));
		}

		[Fact]
		public async Task Update_User()
		{

			UserManagerMock.Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
				.ReturnsAsync(new IdentityUser());

			UserManagerMock.Setup(m => m.UpdateAsync(It.IsAny<IdentityUser>()))
				.ReturnsAsync(IdentityResult.Success);

			UserValidatorMock.Setup(m => m.ValidateAsync(UserManagerMock.Object, It.IsAny<IdentityUser>()))
				.ReturnsAsync(IdentityResult.Success);

			UnitOfWorkMock.Setup(um => um.Customers.UpdateItem(It.IsAny<Customer>())).Verifiable();
			UnitOfWorkMock.Setup(um => um.SaveChanges()).Verifiable();
			var target = new UserController(UserManagerMock.Object, null, null, UserValidatorMock.Object, 
				UnitOfWorkMock.Object, null);

			var resultAction = await target.Create(new UserInfoViewModel()
			{
				Customer = new Customer(),
				Email = "Email@com.com",
				Password = "123",
				IsAdmin = false
			}) as RedirectToActionResult;

			Assert.NotNull(resultAction);
			UnitOfWorkMock.Verify(e => e.SaveChanges());
			UnitOfWorkMock.Verify(e => e.Customers.UpdateItem(It.IsAny<Customer>()));
		}

		[Fact]
		public async Task Create_User_With_Bad_Login()
		{
			UserManagerMock.Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
				.ReturnsAsync(new IdentityUser());

			UserValidatorMock.Setup(m => m.ValidateAsync(UserManagerMock.Object, It.IsAny<IdentityUser>()))
				.ReturnsAsync(IdentityResult.Failed());

			var target = new UserController(UserManagerMock.Object, null, null, UserValidatorMock.Object,
				UnitOfWorkMock.Object, null);

			var resultAction = await target.Create(new UserInfoViewModel()
			{
				Customer = new Customer(),
				Email = "Email@com.com",
				Password = "123",
				IsAdmin = false
			}) as ViewResult;

			Assert.NotNull(resultAction);
		}
	}
}