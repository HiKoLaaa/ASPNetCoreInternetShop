using System.ComponentModel.DataAnnotations;

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