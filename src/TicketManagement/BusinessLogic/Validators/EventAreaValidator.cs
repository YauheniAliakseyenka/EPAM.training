using BusinessLogic.ViewEntities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Validators
{
	internal class EventAreaValidator : AbstractValidator<EventAreaView>
	{
		public static bool IsDescriptionUnique(string description, IEnumerable<EventAreaView> areas)
		{
			if (areas.Count() == 0)
				return true;

			return (areas.Where(y => y.Description.ToLower() == description.ToLower()).Count() == 0);
		}
	}
}
