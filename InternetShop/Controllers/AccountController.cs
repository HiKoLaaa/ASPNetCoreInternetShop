using InternetShop.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetShop.Controllers
{
	[Authorize]
	public class AccountController : Controller
	{
		private UserManager<IdentityUser> _userManager;
		private SignInManager<IdentityUser> _signInManager;

		public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		[AllowAnonymous]
		public IActionResult Login()
		{
			ViewBag.ReturnUrl = RouteData.Values["returnUrl"];
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(UserInfoViewModel info, string returnUrl)
		{
			if (ModelState.IsValid)
			{
				IdentityUser user = await _userManager.FindByEmailAsync(info.Email);
				if (user != null)
				{
					await _signInManager.SignOutAsync();
					Microsoft.AspNetCore.Identity.SignInResult result =
						await _signInManager.PasswordSignInAsync(user, info.Password, false, false);

					if (result.Succeeded)
					{
						return Redirect(returnUrl ?? "/");
					}		
				}

				ModelState.AddModelError("", "Неправильный логин или пароль");
			}

			return View(info);
		}

		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(Login));
		}
	}
}