using BusinessLogic.DTO;
using System.Linq;
using WcfWebHost.Contracts.Data;

namespace WcfWebHost.Helpers.Parsers
{
    internal class LayoutParser
    {
        public static Layout ToLayoutContract(LayoutDto from)
        {
			return new Layout
			{
				Description = from.Description,
				Id = from.Id,
				VenueId = from.VenueId,
				AreaList = from.AreaList.Select(x => AreaParser.ToAreaContract(x)).ToList()
			};
        }

        public static LayoutDto ToLayoutDto(Layout from)
        {
            return new LayoutDto
            {
                Description = from.Description,
                Id = from.Id,
                VenueId = from.VenueId,
                AreaList = from.AreaList.Select(x => AreaParser.ToAreaDto(x)).ToList()
            };
        }
    }
}
