using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using InternetShop.Infrastructure.TagHelpers;

namespace InternetShop.Tests.TagHelpers.Tests
{
	public class ValidateTagHelperTests
	{
		[Fact]
		public void Valid_Output_Tag()
		{
			var tagHelperList = new TagHelperAttributeList();
			var dictItems = new Dictionary<object, object>();
			var context = new TagHelperContext(tagHelperList, dictItems, "uniqueId");
			var output = new TagHelperOutput("div", tagHelperList, (x, en) => 
				Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

			var target = new ValidateTagHelper()
			{
				NeedValidate = true,
				NumbCol = 5
			};

			target.Process(context, output);

			Assert.Equal($"text-danger col-5", output.Attributes["class"].Value);
		}
	}
}