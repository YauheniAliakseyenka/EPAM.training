using System.Collections.ObjectModel;
using System.Linq;
using TicketManagementWPF.VenueService;

namespace TicketManagementWPF.Helpers.Parsers
{
    internal class LayoutParser
    {
        public static Models.Layout ToLayout(Layout from)
        {
            return new Models.Layout
            {
                List = new ObservableCollection<Models.Area>(from.AreaList.Select(x => AreaParser.ToArea(x)).ToList()),
                Description = from.Description,
                Id = from.Id,
                VenueId = from.VenueId
			};
        }

		public static Layout ToLayoutContract(Models.Layout from)
		{
			return new Layout
			{
				AreaList = from.List.Select(x=>AreaParser.ToAreaContract(x)).ToArray(),
				Description = from.Description,
				Id = from.Id,
				VenueId = from.VenueId
			};
		}
	}
}
