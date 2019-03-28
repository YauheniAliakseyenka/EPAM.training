using BusinessLogic.DTO;
using DataAccess.Entities;
using System.Collections.Generic;

namespace BusinessLogic.Parsers
{
	internal static class LayoutParser
	{
		public static LayoutDto MapToLayoutDto(Layout from)
		{
			return new LayoutDto
			{
				AreaList = new List<AreaDto>(),
				Description = from.Description,
				Id = from.Id,
				VenueId = from.VenueId
			};
		}

		public static Layout MapToLayout(LayoutDto from)
		{
			return new Layout
			{
				Description = from.Description,
				VenueId = from.VenueId
			};
		}
	}
}
