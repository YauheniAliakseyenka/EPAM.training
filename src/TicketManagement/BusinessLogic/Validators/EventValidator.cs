using BusinessLogic.ViewEntities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Validators
{
	internal class EventValidator: AbstractValidator<EventView>
	{
		public static bool isDateNotPast(DateTime date)
		{
			if (date <= DateTime.Now)
				return false;

			return true; 
		}

		public static bool isDateValid(EventView obj, IEnumerable<EventView> events)
		{
			if(events.Count() == 0)
				return true;

			return (events.Where(x => x.LayoutId == obj.LayoutId & x.Date == obj.Date).Count() == 0);
		}
	}
}
