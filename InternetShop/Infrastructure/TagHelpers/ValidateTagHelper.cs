using Microsoft.AspNetCore.Razor.TagHelpers;

namespace InternetShop.Infrastructure.TagHelpers
{
	[HtmlTargetElement("div", Attributes = "danger-validation, numb-col")]
	public class ValidateTagHelper : TagHelper
	{
		[HtmlAttributeName("danger-validation")]
		public bool NeedValidate { get; set; }

		public int NumbCol { get; set; }

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			if (NeedValidate)
			{
				output.PreElement.SetHtmlContent("<div class=\"row justify-content-center\">");
				output.Attributes.SetAttribute("class", $"text-danger col-{NumbCol}");
				output.PostElement.SetHtmlContent("</div>");
			}
		}
	}
}