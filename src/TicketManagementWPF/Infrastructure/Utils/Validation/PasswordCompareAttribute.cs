using System;
using System.ComponentModel.DataAnnotations;
using System.Security;
using TicketManagementWPF.Infrastructure.Extensions;

namespace TicketManagementWPF.Infrastructure.Utils.Validation
{
	internal class PasswordCompareAttribute : ValidationAttribute
	{
		private readonly string _propertyToValidate;

		public PasswordCompareAttribute(string propertyName)
		{
			_propertyToValidate = propertyName;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var property = validationContext.ObjectType.GetProperty(_propertyToValidate);

			if(property is null)
				return new ValidationResult("Property was not found");

			var propValue = property.GetValue(validationContext.ObjectInstance);

			if(propValue is null)
				return null;

			if (!(propValue is SecureString str))
				return new ValidationResult("Property type must be SecureString");

			if (str.GetStringValue().Equals((string)value, StringComparison.Ordinal))
				return ValidationResult.Success;

			return new ValidationResult(ErrorMessage);
		}
	}
}
