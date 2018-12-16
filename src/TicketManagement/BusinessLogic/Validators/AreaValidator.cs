using BusinessLogic.ViewEntities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Validators
{
	internal class AreaValidator : AbstractValidator<AreaView>
	{
		public static bool IsDescriptionUnique(string description, IEnumerable<AreaView> areas)
		{
			if (areas.Count() == 0)
				return true;

			return (areas.Where(y => y.Description.ToLower() == description.ToLower()).Count() == 0);
		}
	}
}
