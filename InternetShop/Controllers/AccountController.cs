using InternetShop.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InternetShop.Controllers
{
	[Authorize]
	public class AccountController : Controller
	{
		private UserManager<IdentityUser> _userManager;
		private SignInManager<IdentityUser> _signInManager;
		private IPasswordValidator<IdentityUser> _passwordValidator;

		public AccountController(UserManager<IdentityUser> userManager, 
			SignInManager<IdentityUser> signInManager,
			IPasswordValidator<IdentityUser> passwordValidator)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_passwordValidator = passwordValidator;
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


		public IActionResult ChangePassword() => View();

		[HttpPost]
		public async Task<IActionResult> ChangePassword(PasswordChangeViewModel pcVM)
		{
			if (!ModelState.IsValid)
			{
				return View(pcVM);
			}

			IdentityUser currUser = await _userManager.GetUserAsync(User);
			bool truePassword = await _userManager.CheckPasswordAsync(currUser, pcVM.OldPassword);
			if (!truePassword)
			{
				ModelState.AddModelError("", "Неправильный старый пароль");
				return View(pcVM);
			}

			IdentityResult result = await _passwordValidator.ValidateAsync(_userManager, currUser, pcVM.NewPassword);
			if (!result.Succeeded)
			{
				ModelState.AddModelError("", "Новый пароль не соответствует требованиям");
				return View(pcVM);
			}

			if (pcVM.NewPassword != pcVM.RepeatNewPassword)
			{
				ModelState.AddModelError("", "Пароли не совпадают");
				return View(pcVM);
			}

			result = await _userManager.ChangePasswordAsync(currUser, pcVM.OldPassword, pcVM.NewPassword);
			if (!result.Succeeded)
			{
				ModelState.AddModelError("", "Произошла ошибка, повторите попытку");
				return View(pcVM);
			}

			if (ModelState.IsValid)
			{
				return RedirectToAction("Index", "PersonalAccount");
			}
			else
			{
				return View(pcVM);
			}
		}
	}
}