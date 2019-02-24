using BusinessLogic.DTO;
using WcfWebHost.Contracts.Data;

namespace WcfWebHost.Helpers.Parsers
{
	internal class OrderParser
	{
		public static Order ToOrderContract(OrderDto from)
		{
			return new Order
			{
				Date = from.Date,
				Id = from.Id,
				UserId = from.UserId
			};
		}

		public static OrderDto ToOrderDto(Order from)
		{
			return new OrderDto
			{
				Date = from.Date,
				Id = from.Id,
				UserId = from.UserId
			};
		}
	}
}
