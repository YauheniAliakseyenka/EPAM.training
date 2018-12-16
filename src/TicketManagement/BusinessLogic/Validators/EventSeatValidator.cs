using BusinessLogic.ViewEntities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Validators
{
	internal class EventSeatValidator : AbstractValidator<EventSeatView>
	{
		public static bool isSeatUnique(EventSeatView seat, IEnumerable<EventSeatView> seats)
		{
			if (seats.Count() == 0)
				return true;

			return (seats.Where(c => c.Row == seat.Row & c.Number == seat.Number).Count() == 0);
		}
	}
}
