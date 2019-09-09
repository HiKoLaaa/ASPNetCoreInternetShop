using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternetShop.Models.ViewModels
{
	public class UserInfo
	{
		[Display(Name = "Имя")]
		public string Name { get; set; }

		[Required]
		[Display(Name = "Электронная почта")]
		[UIHint("email")]
		public string Email { get; set; }

		[Required]
		[Display(Name = "Пароль")]
		[UIHint("password")]
		public string Password { get; set; }
	}
}