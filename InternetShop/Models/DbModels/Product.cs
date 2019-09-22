using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InternetShop.Models.DbModels
{
	public class Product
	{
		[Key]
		public Guid ID { get; set; }

		[Display(Name = "Код")]
		[RegularExpression(@"\d{2}-\d{4}-[A-Z]{2}\d{2}",
			ErrorMessage = "Код должен соответстовать шаблону \"XX-XXXX-YYXX\", где X - цифра, а Y - заглавная буква " +
				"английского алфавита")]
		public string Code { get; set; }

		[Display(Name = "Название")]
		public string Name { get; set; }

		[Display(Name = "Цена")]
		public decimal Price { get; set; }

		[StringLength(25)]
		[Display(Name = "Категория")]
		public string Category { get; set; }

		[JsonIgnore]
		public List<OrderProduct> Orders { get; set; }

		public Product()
		{
			Orders = new List<OrderProduct>();
		}
	}
}