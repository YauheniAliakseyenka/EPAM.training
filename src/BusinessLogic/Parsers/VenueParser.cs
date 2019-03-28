using BusinessLogic.DTO;
using DataAccess.Entities;
using System.Collections.Generic;

namespace BusinessLogic.Parsers
{
	internal static class VenueParser
	{
		public static VenueDto MapToVenueDto(Venue from)
		{
			return new VenueDto
			{
				Address = from.Address,
				Description = from.Description,
				Id = from.Id,
				Name = from.Name,
				Phone = from.Phone,
				Timezone = from.Timezone,
				LayoutList = new List<LayoutDto>()
			};
		}

		
		public static Venue MapToVenue(VenueDto from)
		{
			return new Venue
			{
				Address = from.Address,
				Description = from.Description,
				Name = from.Name,
				Phone = from.Phone,
				Timezone = from.Timezone
			};
		}
	}
}
