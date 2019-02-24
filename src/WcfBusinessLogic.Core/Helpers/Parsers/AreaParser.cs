using BusinessLogic.DTO;
using System.Linq;
using WcfBusinessLogic.Core.Contracts.Data;

namespace WcfBusinessLogic.Core.Helpers.Parsers
{
    internal class AreaParser
    {
        public static Area ToAreaContract(AreaDto from)
        {
            return new Area
            {
                SeatList = from.SeatList?.Select(x=>SeatParser.ToSeatContract(x)).ToList(),
                CoordX  = from.CoordX,
                CoordY = from.CoordY,
                Description = from.Description,
                Id = from.Id,
                LayoutId =from.LayoutId
            };
        }

        public static AreaDto ToAreaDto(Area from)
        {
            return new AreaDto
            {
                SeatList = from.SeatList?.Select(x => SeatParser.ToSeatDto(x)).ToList(),
                CoordX = from.CoordX,
                CoordY = from.CoordY,
                Description = from.Description,
                Id = from.Id,
                LayoutId = from.LayoutId
            };
        }
	}
}
