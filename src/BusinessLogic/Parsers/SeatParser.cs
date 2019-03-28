using BusinessLogic.DTO;
using DataAccess.Entities;

namespace BusinessLogic.Parsers
{
	internal static class SeatParser
	{
		public static SeatDto MapToSeatDto(Seat from)
		{
			return new SeatDto
			{
				AreaId = from.AreaId,
				Id = from.Id,
				Number = from.Number,
				Row = from.Row
			};
		}

		public static Seat MapToSeat(SeatDto from)
		{
			return new Seat
			{
				AreaId = from.AreaId,
				Number = from.Number,
				Row = from.Row
			};
		}
	}
}
