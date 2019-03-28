using BusinessLogic.DTO;
using DataAccess.Entities;
using System.Collections.Generic;

namespace BusinessLogic.Parsers
{
	internal static class AreaParser
	{
		public static AreaDto MapToAreaDto(Area from)
		{
			return new AreaDto
			{
				SeatList = new List<SeatDto>(),
				CoordX = from.CoordX,
				CoordY = from.CoordY,
				Description = from.Description,
				Id = from.Id,
				LayoutId = from.LayoutId
			};
		}

		public static Area MapToArea(AreaDto from)
		{
			return new Area
			{
				CoordX = from.CoordX,
				CoordY = from.CoordY,
				Description = from.Description,
				LayoutId = from.LayoutId
			};
		}
	}
}
