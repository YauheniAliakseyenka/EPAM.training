using BusinessLogic.ViewEntities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Validators
{
	internal class SeatValdiator: AbstractValidator<SeatView>
	{
		public static bool isSeatUnique(SeatView seat, IEnumerable<SeatView> seats)
		{
			if (seats.Count() == 0)
				return true;

			return (seats.Where(c => c.Row == seat.Row & c.Number == seat.Number).Count() == 0);
		}
	}
}
