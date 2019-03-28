using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Security;
using TicketManagementWPF.Infrastructure.Extensions;

namespace TicketManagementWPF.Infrastructure.Validation.Utils
{
	internal static class AttributeValidator
	{
		public static Dictionary<string, List<string>>  Validate(object model)
		{
			var validationResults = new Dictionary<string, List<string>>();

			//get properties with validation attributes
			var properties = model.GetType().GetProperties().Where(x => x.GetCustomAttributes<ValidationAttribute>().Any()).ToList();

			if (!properties.Any())
				return validationResults;

			foreach (var property in properties)
			{
				var results = new List<string>();
				var tempValue = property.GetValue(model);

				object value = tempValue;
				if (tempValue != null && tempValue.GetType() == typeof(SecureString))
					value = (tempValue as SecureString)?.GetStringValue();
				else
					value = tempValue;

				var validationContext = new ValidationContext(model);
				foreach (var validationAttribute in property.GetCustomAttributes<ValidationAttribute>())
				{
					var result = validationAttribute.GetValidationResult(value, validationContext);

					if (result != null)
						results.Add(result.ErrorMessage);
				}

				if(results.Any())
					validationResults[property.Name] = results;
			}

			return validationResults;
		}

		public static IEnumerable<string> GetOnlyErrors(object model)
		{
			var dictionary = Validate(model);

			if (!dictionary.Any())
				return Enumerable.Empty<string>();

			var list = new List<string>();
			foreach (var property in dictionary)
				foreach (var error in property.Value)
					list.Add(error);

			return list;
		}
	}
}
