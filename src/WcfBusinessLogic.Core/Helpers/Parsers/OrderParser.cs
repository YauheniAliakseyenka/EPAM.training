using BusinessLogic.DTO;
using WcfBusinessLogic.Core.Contracts.Data;

namespace WcfBusinessLogic.Core.Helpers.Parsers
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
