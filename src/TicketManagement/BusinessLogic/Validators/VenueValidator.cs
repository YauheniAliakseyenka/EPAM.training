using BusinessLogic.ViewEntities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Validators
{
	internal class VenueValidator : AbstractValidator<VenueView>
	{
		public static bool isNameUnique(string name, IEnumerable<VenueView> venues)
		{
			if (venues.Count() == 0)
				return true;

			return (venues.Select(x => x.Name).Where(y => y.ToLower() == name.ToLower()).Count() == 0);
		}
	}
}
