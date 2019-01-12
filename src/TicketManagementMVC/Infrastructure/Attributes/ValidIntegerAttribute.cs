using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TicketManagementMVC.Infrastructure.Attributes
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property)]
	internal class ValidIntegerAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (value == null || value.ToString().Length == 0)
			{
				return ValidationResult.Success;
			}
			int i;

			return !int.TryParse(value.ToString(), out i) ? new ValidationResult(ErrorMessage) : ValidationResult.Success;
		}
	}

	internal class ValidIntegerValidator : DataAnnotationsModelValidator<ValidIntegerAttribute>
	{
		public ValidIntegerValidator(ModelMetadata metadata, ControllerContext context, ValidIntegerAttribute attribute)
			: base(metadata, context, attribute)
		{
			if (!attribute.IsValid(context.HttpContext.Request.Form[metadata.PropertyName]))
			{
				var propertyName = metadata.PropertyName;
				context.Controller.ViewData.ModelState[propertyName].Errors.Clear();
				context.Controller.ViewData.ModelState[propertyName].Errors.Add(string.Format(I18N.ResourceErrors.NumericError, propertyName));
			}
		}
	}
}