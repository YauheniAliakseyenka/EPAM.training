using BusinessLogic.ViewEntities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Validators
{
	internal class LayoutValidator : AbstractValidator<LayoutView>
	{
		public static bool IsDescriptionUnique(string description, IEnumerable<LayoutView> layouts)
		{
			if (layouts.Count() == 0)
				return true;

			return (layouts.Where(y => y.Description.ToLower() == description.ToLower()).Count() == 0);
		}
	}
}
