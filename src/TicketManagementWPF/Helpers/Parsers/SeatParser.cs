using TicketManagementWPF.VenueService;

namespace TicketManagementWPF.Helpers.Parsers
{
    internal class SeatParser
    {
        public static Models.Seat ToSeat(Seat from)
        {
            return new Models.Seat
            {
                AreaId = from.AreaId,
                Id = from.Id,
                Column = from.Number,
                Row = from.Row
            };
        }

		public static Seat ToSeatContract(Models.Seat from)
		{
			return new Seat
			{
				AreaId = from.AreaId,
				Id = from.Id,
				Number = from.Column,
				Row = from.Row
			};
		}
	}
}
