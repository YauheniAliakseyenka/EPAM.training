using BusinessLogic.DTO;
using System.Linq;
using System.ServiceModel;
using WcfWebHost.Contracts.Data;
using WcfWebHost.FaultExceptions;

namespace WcfWebHost.Helpers.Parsers
{
    internal class EventAreaParser
    {
        public static EventArea ToEventAreaContract(EventAreaDto from)
        {
			if (from.Seats == null)
				ThrowSeatListException();

			return new EventArea
			{
				Seats = from.Seats.Select(x =>
				{
					if (x == null)
						ThrowSeatListException();

					return EventSeatParser.ToEventSeatContract(x);
				}).ToList(),
				AreaDefaultId = from.AreaDefaultId,
				CoordX = from.CoordX,
				CoordY = from.CoordY,
				Description = from.Description,
				Price = from.Price,
				Id = from.Id,
				EventId = from.EventId
			};
        }

		public static EventAreaDto ToEventAreaDto(EventArea from)
		{
			if (from.Seats == null)
				ThrowSeatListException();

			return new EventAreaDto
			{
				Seats = from.Seats.Select(x =>
				{
					if (x == null)
						ThrowSeatListException();

					return EventSeatParser.ToEventSeatDto(x);
				}).ToList(),
				AreaDefaultId = from.AreaDefaultId,
				CoordX = from.CoordX,
				CoordY = from.CoordY,
				Description = from.Description,
				Price = from.Price,
				Id = from.Id,
				EventId = from.EventId
			};
		}


		private static void ThrowSeatListException()
		{
			var fault = new EventAreaFault { Message = "Event area mapping error" };
			var reason = "Invalid state of event area. Seat list is empty";
			throw new FaultException<EventAreaFault>(fault, reason);
		}
    }
}
