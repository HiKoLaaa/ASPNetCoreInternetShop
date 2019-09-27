using InternetShop.Models.DbModels;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace InternetShop.Infrastructure.Attributes
{
	public class TrueCustomerCodeAttribute : Attribute, IModelValidator
	{
		public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
		{
			string validateCode = context.Model as string;
			Regex regex = new Regex(@"\d{4}-\d{4}");
			if (!string.IsNullOrEmpty(validateCode) && regex.IsMatch(validateCode))
			{
				int year = int.Parse(validateCode.Substring(5));
				if (year >= DateTime.Now.Year)
				{
					return Enumerable.Empty<ModelValidationResult>();
				}
			}

			return new List<ModelValidationResult>()
			{
				new ModelValidationResult(nameof(Product.Code),
					"Код должен соответстовать шаблону «ХХХХ-ГГГГ» где Х – число, " +
					"ГГГГ – год в котором зарегистрирован заказчик (больше или равен текущему году).")
			};
		}
	}
}