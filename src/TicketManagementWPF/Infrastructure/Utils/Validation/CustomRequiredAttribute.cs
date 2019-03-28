using System;
using System.ComponentModel.DataAnnotations;
using System.Resources;

namespace TicketManagementWPF.Infrastructure.Utils.Validation
{
	internal class CustomRequiredAttribute : RequiredAttribute
	{
		private readonly string _displayFieldResourceName;
		private readonly Type _displayFieldResourceType;

		public CustomRequiredAttribute(string displayFieldResourceName, Type displayFieldResourceType)
		{
			_displayFieldResourceName = displayFieldResourceName;
			_displayFieldResourceType = displayFieldResourceType;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var validationResult = base.IsValid(value, validationContext);

			if (validationResult is null)
				return validationResult;

			var propertyName = new ResourceManager(_displayFieldResourceType).GetString(_displayFieldResourceName);
			var errorText = l10n.Shared.Errors.PropertyRequired;

			return new ValidationResult(string.Format(errorText, propertyName));
		}
	}
}
