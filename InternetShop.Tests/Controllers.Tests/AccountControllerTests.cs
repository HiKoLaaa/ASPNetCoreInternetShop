using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using InternetShop.Controllers;
using InternetShop.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace InternetShop.Tests.Controllers.Tests
{
	public class AccountControllerTests
	{
		public Mock<UserManager<IdentityUser>> UserManagerMock { get; set; }
		public Mock<SignInManager<IdentityUser>> SignInManagerMock { get; set; }
		public Mock<IPasswordValidator<IdentityUser>> PasswordValidatorMock { get; set; }

		public AccountControllerTests()
		{
			var userStoreMock = new Mock<IUserStore<IdentityUser>>();
			UserManagerMock = new Mock<UserManager<IdentityUser>>(userStoreMock.Object,
				null, null, null, null, null, null, null, null);

			PasswordValidatorMock = new Mock<IPasswordValidator<IdentityUser>>();
			var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
			var userClaimsPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<IdentityUser>>();
			SignInManagerMock = new Mock<SignInManager<IdentityUser>>(UserManagerMock.Object, 
				httpContextAccessorMock.Object,
				userClaimsPrincipalFactoryMock.Object,
				null, null, null, null);
		}

		[Fact]
		public async Task Invalid_Password_Or_User_Name_Login()
		{
			UserManagerMock.Setup(m => m.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(new IdentityUser());
			SignInManagerMock.Setup(m => m.SignOutAsync());
			SignInManagerMock.Setup(m => m.PasswordSignInAsync(It.IsAny<IdentityUser>(), It.IsAny<string>(), false, false))
				.ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

			var target = new AccountController(UserManagerMock.Object, 
				SignInManagerMock.Object, PasswordValidatorMock.Object);

			var result = await target.Login(new UserInfoViewModel(), "returnUrl") as ViewResult;

			Assert.NotNull(result);
			Assert.Equal(1, target.ModelState.ErrorCount);
		}

		[Fact]
		public async Task Valid_Login()
		{
			UserManagerMock.Setup(m => m.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(new IdentityUser());
			SignInManagerMock.Setup(m => m.SignOutAsync());
			SignInManagerMock.Setup(m => m.PasswordSignInAsync(It.IsAny<IdentityUser>(), It.IsAny<string>(), false, false))
				.ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

			var target = new AccountController(UserManagerMock.Object,
				SignInManagerMock.Object, PasswordValidatorMock.Object);

			var result = await target.Login(new UserInfoViewModel(), "returnUrl") as RedirectResult;
			
			Assert.NotNull(result);
			Assert.Equal("returnUrl", result.Url);
		}

		[Fact]
		public async Task Change_Password_Invalid_Old()
		{
			UserManagerMock.Setup(m => m.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new IdentityUser());
			UserManagerMock.Setup(m => m.CheckPasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
				.ReturnsAsync(false);

			var target = new AccountController(UserManagerMock.Object,
				SignInManagerMock.Object, PasswordValidatorMock.Object);

			var result = await target.ChangePassword(new PasswordChangeViewModel()) as ViewResult;

			Assert.NotNull(result);
			Assert.False(target.ModelState.IsValid);
			Assert.True(target.ModelState[""].Errors[0].ErrorMessage == "Неправильный старый пароль");
		}

		[Fact]
		public async Task Change_Password_Invalid_Coincidence()
		{
			UserManagerMock.Setup(m => m.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new IdentityUser());
			UserManagerMock.Setup(m => m.CheckPasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
				.ReturnsAsync(true);

			var target = new AccountController(UserManagerMock.Object,
				SignInManagerMock.Object, PasswordValidatorMock.Object);

			var result = await target.ChangePassword(new PasswordChangeViewModel()
			{
				NewPassword = "newPassword",
				OldPassword = "oldPassword"
			}) as ViewResult;

			Assert.NotNull(result);
			Assert.False(target.ModelState.IsValid);
			Assert.True(target.ModelState[""].Errors[0].ErrorMessage == "Пароли не совпадают");
		}

		[Fact]
		public async Task Change_Password_Bad_New()
		{
			UserManagerMock.Setup(m => m.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new IdentityUser());
			UserManagerMock.Setup(m => m.CheckPasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
				.ReturnsAsync(true);

			PasswordValidatorMock.Setup(m => m.ValidateAsync(UserManagerMock.Object,
				It.IsAny<IdentityUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed());

			var target = new AccountController(UserManagerMock.Object,
				SignInManagerMock.Object, PasswordValidatorMock.Object);

			var result = await target.ChangePassword(new PasswordChangeViewModel()
			{
				NewPassword = "Password",
				RepeatNewPassword = "Password"
			}) as ViewResult;

			Assert.NotNull(result);
			Assert.False(target.ModelState.IsValid);
			Assert.True(target.ModelState[""].Errors[0].ErrorMessage == "Новый пароль не соответствует требованиям");
		}

		[Fact]
		public async Task Change_Password_Some_Error()
		{
			UserManagerMock.Setup(m => m.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new IdentityUser());
			UserManagerMock.Setup(m => m.CheckPasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
				.ReturnsAsync(true);

			UserManagerMock.Setup(m => m.ChangePasswordAsync(It.IsAny<IdentityUser>(), 
				It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed());

			PasswordValidatorMock.Setup(m => m.ValidateAsync(UserManagerMock.Object,
				It.IsAny<IdentityUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

			var target = new AccountController(UserManagerMock.Object,
				SignInManagerMock.Object, PasswordValidatorMock.Object);

			var result = await target.ChangePassword(new PasswordChangeViewModel()
			{
				NewPassword = "Password",
				RepeatNewPassword = "Password"
			}) as ViewResult;

			Assert.NotNull(result);
			Assert.False(target.ModelState.IsValid);
			Assert.True(target.ModelState[""].Errors[0].ErrorMessage == "Произошла ошибка, повторите попытку");
		}

		[Fact]
		public async Task Change_Password_Valid()
		{
			UserManagerMock.Setup(m => m.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new IdentityUser());
			UserManagerMock.Setup(m => m.CheckPasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
				.ReturnsAsync(true);

			UserManagerMock.Setup(m => m.ChangePasswordAsync(It.IsAny<IdentityUser>(),
				It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

			PasswordValidatorMock.Setup(m => m.ValidateAsync(UserManagerMock.Object,
				It.IsAny<IdentityUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

			var target = new AccountController(UserManagerMock.Object,
				SignInManagerMock.Object, PasswordValidatorMock.Object);

			var result = await target.ChangePassword(new PasswordChangeViewModel()
			{
				NewPassword = "Password",
				RepeatNewPassword = "Password"
			}) as RedirectToActionResult;

			Assert.NotNull(result);
			Assert.True(target.ModelState.IsValid);
		}
	}
}