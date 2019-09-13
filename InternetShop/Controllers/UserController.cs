using InternetShop.Models.Repository;
using InternetShop.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetShop.Controllers
{
	public class UserController : Controller
	{
		private UserManager<IdentityUser> _userManager;
		private IPasswordValidator<IdentityUser> _passwordValidator;
		private IPasswordHasher<IdentityUser> _passwordHasher;
		private IUserValidator<IdentityUser> _userValidator;

		public UserController(UserManager<IdentityUser> userManager,
			IPasswordValidator<IdentityUser> passwordValidator,
			IPasswordHasher<IdentityUser> passwordHasher,
			IUserValidator<IdentityUser> userValidator)
		{
			_passwordValidator = passwordValidator;
			_passwordHasher = passwordHasher;
			_userValidator = userValidator;
			_userManager = userManager;
		}

		public IActionResult AllUsers() => View(_userManager.Users);

		[HttpPost]
		public async Task<IActionResult> Delete(Guid id)
		{
			IdentityUser user = await _userManager.FindByIdAsync(id.ToString());
			IdentityResult result = await _userManager.DeleteAsync(user);
			return RedirectToAction(nameof(AllUsers));
		}

		public IActionResult Create()
		{
			ViewBag.Action = "Добавление нового пользователя";
			return View();
		}

		public async Task<IActionResult> Edit(Guid id)
		{
			ViewBag.Action = "Редактирование пользователя";
			IdentityUser user = await _userManager.FindByIdAsync(id.ToString());
			ModelState.AddModelError(nameof(UserInfoViewModel.Password), "Пароль необходимо ввести заново или новый");
			return View(nameof(Create),
				new UserInfoViewModel()
				{
					Email = user.Email,
					Name = user.UserName
				}
			);
		}

		[HttpPost]
		public async Task<IActionResult> Create(UserInfoViewModel userInfo)
		{
			IdentityUser newUser = await _userManager.FindByEmailAsync(userInfo.Email);
			IdentityResult result;
			if (newUser != null)
			{
				newUser.Email = userInfo.Email;
				newUser.UserName = userInfo.Name;
				result = await _userValidator.ValidateAsync(_userManager, newUser);
				if (!result.Succeeded)
				{
					// TODO: добавить описание верного логина.
					ModelState.AddModelError("", "Недопустимый логин");
					return View(userInfo);
				}
				result = await _passwordValidator.ValidateAsync(_userManager, newUser, userInfo.Password);
				if (!result.Succeeded)
				{
					ModelState.AddModelError("", "Недопустимый пароль (длина минимум 6 символов, заглавные буквы + цифры");
					return View(userInfo);
				}

				newUser.PasswordHash = _passwordHasher.HashPassword(newUser, userInfo.Password);
				result = await _userManager.UpdateAsync(newUser);
				if (!result.Succeeded)
				{
					ModelState.AddModelError("", "При создании произошла ошибка, повторите попытку");
					return View(userInfo);
				}
			}

			newUser = new IdentityUser
			{
				Email = userInfo.Email,
				UserName = userInfo.Name
			};

			result = await _userManager.CreateAsync(newUser, userInfo.Password);
			if (!result.Succeeded)
			{
				ModelState.AddModelError("", "При создании произошла ошибка, повторите попытку");
				return View(userInfo);
			}

			return RedirectToAction(nameof(AllUsers));
		}
	}
}