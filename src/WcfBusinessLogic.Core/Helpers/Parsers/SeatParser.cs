using BusinessLogic.DTO;
using WcfBusinessLogic.Core.Contracts.Data;

namespace WcfBusinessLogic.Core.Helpers.Parsers
{
    internal class SeatParser
    {
        public static Seat ToSeatContract(SeatDto from)
        {
            return new Seat
            {
               AreaId = from.AreaId,
               Id = from.Id,
               Number = from.Number,
               Row = from.Row
			};
        }

        public static SeatDto ToSeatDto(Seat from)
        {
            return new SeatDto
            {
                AreaId = from.AreaId,
                Id = from.Id,
                Number = from.Number,
                Row = from.Row
			};
        }
    }
}
