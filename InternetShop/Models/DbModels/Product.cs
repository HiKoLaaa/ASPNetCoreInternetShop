using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternetShop.Models.DbModels
{
	public class Product
	{
		[Key]
		public Guid ID { get; set; }

		[Required]
		[Display(Name = "Код")]
		// TODO: [RegularExpression()]
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
	}
}
