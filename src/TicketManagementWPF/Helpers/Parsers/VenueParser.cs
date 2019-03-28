using System.Collections.ObjectModel;
using System.Linq;
using TicketManagementWPF.VenueService;

namespace TicketManagementWPF.Helpers.Parsers
{
    internal class VenueParser
    {
        public static Models.Venue ToVenue(Venue from)
        {
            return new Models.Venue
            {
                Address = from.Address,
                Description = from.Description,
                Id = from.Id,
                LayoutList = new ObservableCollection<Models.Layout>(from.LayoutList.Select(x => LayoutParser.ToLayout(x)).ToList()),
                Name = from.Name,
                Phone = from.Phone,
                Timezone = from.Timezone
            };
        }

		public static Venue ToVenueContract(Models.Venue from)
		{
			return new Venue
			{
				Address = from.Address,
				Description = from.Description,
				Id = from.Id,
				LayoutList = from.LayoutList.Select(x=>LayoutParser.ToLayoutContract(x)).ToArray(),
				Name = from.Name,
				Phone = from.Phone,
				Timezone = from.Timezone
			};
		}
	}
}
