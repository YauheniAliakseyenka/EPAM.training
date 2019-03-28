using System.Collections.ObjectModel;
using System.Linq;
using TicketManagementWPF.VenueService;

namespace TicketManagementWPF.Helpers.Parsers
{
    internal class AreaParser
    {
        public static Models.Area ToArea(Area from)
        {
            return new Models.Area
            {
                List = new ObservableCollection<Models.Seat>(from.SeatList.Select(x => SeatParser.ToSeat(x)).ToList()),
                Column = from.CoordX,
                Row = from.CoordY,
                Description = from.Description,
                Id = from.Id,
                LayoutId = from.LayoutId
			};
        }

		public static Area ToAreaContract(Models.Area from)
		{
			return new Area
			{
				SeatList = from.List.Select(x=>SeatParser.ToSeatContract(x)).ToArray(),
				CoordX = from.Column,
				CoordY = from.Row,
				Description = from.Description,
				Id = from.Id,
				LayoutId = from.LayoutId
			};
		}
	}
}
