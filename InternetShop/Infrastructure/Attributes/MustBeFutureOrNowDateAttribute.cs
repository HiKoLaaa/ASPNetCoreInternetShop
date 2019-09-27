using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InternetShop.Infrastructure.Attributes
{
	public class MustBeFutureOrNowDateAttribute : Attribute, IModelValidator
	{
		public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
		{
			DateTime? dt = context.Model as DateTime?;
			if (!dt.HasValue || dt.Value < DateTime.Today)
			{
				return new List<ModelValidationResult>()
				{
					new ModelValidationResult("", "Дата должна быть больше или равна текущей")
				};
			}
			else
			{
				return Enumerable.Empty<ModelValidationResult>();
			}
		}
	}
}