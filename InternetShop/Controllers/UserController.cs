using InternetShop.Models.DbModels;
using InternetShop.Models.UnitOfWork;
using InternetShop.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace InternetShop.Controllers
{
	[Authorize(Roles = "Admin")]
	public class UserController : Controller
	{
		private UserManager<IdentityUser> _userManager;
		private RoleManager<IdentityRole> _roleManager;
		private IUnitOfWork _unitOfWork;
		private IPasswordValidator<IdentityUser> _passwordValidator;
		private IPasswordHasher<IdentityUser> _passwordHasher;
		private IUserValidator<IdentityUser> _userValidator;

		public UserController(UserManager<IdentityUser> userManager,
			IPasswordValidator<IdentityUser> passwordValidator,
			IPasswordHasher<IdentityUser> passwordHasher,
			IUserValidator<IdentityUser> userValidator,
			IUnitOfWork unitOfWork,
			RoleManager<IdentityRole> roleManager)
		{
			_passwordValidator = passwordValidator;
			_passwordHasher = passwordHasher;
			_userValidator = userValidator;
			_userManager = userManager;
			_unitOfWork = unitOfWork;
			_roleManager = roleManager;
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
					Customer = _unitOfWork.Customers.GetItem(id)
				});
		}

		[HttpPost]
		public async Task<IActionResult> Create(UserInfoViewModel userInfo)
		{
			// TODO: сделать возможность администраторам добавлять других администраторов.
			if (!ModelState.IsValid)
			{
				if (ModelState.ErrorCount == 1 &&
					ModelState.GetValidationState(nameof(Customer.ID)) == ModelValidationState.Invalid)
				{
					return View(userInfo);
				}
			}

			IdentityUser newUser = await _userManager.FindByEmailAsync(userInfo.Email);
			IdentityResult result;
			if (newUser != null)
			{
				newUser.Email = userInfo.Email;
				newUser.UserName = userInfo.Customer.Name;
				result = await _userValidator.ValidateAsync(_userManager, newUser);
				if (!result.Succeeded)
				{
					ModelState.AddModelError("", "Недопустимый логин (должен состоять из букв латинского алфавита +" +
						"некоторые специальные символы: \"-._+@\")");

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

				_unitOfWork.Customers.UpdateItem(userInfo.Customer);
			}
			else
			{
				newUser = new IdentityUser
				{
					Email = userInfo.Email,
					UserName = userInfo.Customer.Name
				};

				result = await _userManager.CreateAsync(newUser, userInfo.Password);
				if (!result.Succeeded)
				{
					foreach (var err in result.Errors)
					{
						ModelState.AddModelError("", err.Description);
					}

					return View(userInfo);
				}

				Customer newCustomer = userInfo.Customer;
				IdentityUser user = await _userManager.FindByEmailAsync(userInfo.Email);
				newCustomer.ID = Guid.Parse(user.Id);
				_unitOfWork.Customers.AddItem(userInfo.Customer);
			}

			_unitOfWork.SaveChanges();
			return RedirectToAction(nameof(AllUsers));
		}
	}
}