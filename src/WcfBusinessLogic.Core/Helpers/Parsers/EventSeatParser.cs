using BusinessLogic.DTO;
using WcfBusinessLogic.Core.Contracts.Data;

namespace WcfBusinessLogic.Core.Helpers.Parsers
{
    internal class EventSeatParser
    {
        public static EventSeat ToEventSeatContract(EventSeatDto from)
        {
            return new EventSeat
            {
               State = (SeatState)SeatStateEnumParser.ToSeatStateContract(from.State),
               Row = from.Row,
               Number = from.Number,
               Id = from.Id,
               EventAreaId = from.EventAreaId
            };
        }

        public static EventSeatDto ToEventSeatDto(EventSeat from)
        {
            return new EventSeatDto
            {
                State =(BusinessLogic.Services.SeatState)SeatStateEnumParser.ToSeatStateBLL(from.State),
                Row = from.Row,
                Number = from.Number,
                Id = from.Id,
                EventAreaId = from.EventAreaId
            };
        }
    }
}
