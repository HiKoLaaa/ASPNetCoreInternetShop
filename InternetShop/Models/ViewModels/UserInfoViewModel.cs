﻿using InternetShop.Models.DbModels;
using System.ComponentModel.DataAnnotations;

namespace InternetShop.Models.ViewModels
{
	public class UserInfoViewModel
	{
		public Customer Customer { get; set; }

		[Required]
		[Display(Name = "Электронная почта")]
		[UIHint("email")]
		public string Email { get; set; }

		[Required]
		[Display(Name = "Пароль")]
		[UIHint("password")]
		public string Password { get; set; }

		[Display(Name = "Администратор")]
		public bool IsAdmin { get; set; }
	}
}