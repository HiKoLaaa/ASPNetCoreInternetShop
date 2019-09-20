using InternetShop.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternetShop.Models.DbModels
{
	public class Customer
	{
		[Key]
		public Guid ID { get; set; }

		[Required]
		[Display(Name = "Имя")]
		public string Name { get; set; }

		[Required]
		[TrueCustomerCode]
		[Display(Name = "Код")]
		public string Code { get; set; }

		[Required]
		[Display(Name = "Адрес")]
		public string Address { get; set; }

		[Required]
		[Display(Name = "Скидка")]
		public int Discount { get; set; }

		public List<Order> Orders { get; set; }

		public Customer()
		{
			Orders = new List<Order>();
		}
	}
}