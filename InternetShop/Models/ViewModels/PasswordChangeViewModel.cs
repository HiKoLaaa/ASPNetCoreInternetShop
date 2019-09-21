using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternetShop.Models.ViewModels
{
	public class PasswordChangeViewModel
	{
		[Required(ErrorMessage = "Введите старый пароль")]
		public string OldPassword { get; set; }

		[Required(ErrorMessage = "Введите новый пароль")]
		public string NewPassword { get; set; }

		[Required(ErrorMessage = "Повторите новый пароль")]
		public string RepeatNewPassword { get; set; }
	}
}