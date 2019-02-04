using System;
using System.Collections.Generic;

namespace BusinessLogic.DTO
{
	public class LayoutDto
	{
		public int Id { get; set; }
		public int VenueId { get; set; }
		public string Description { get; set; }
		public List<AreaDto> AreaList { get; set; }

		public LayoutDto()
		{
			AreaList = new List<AreaDto>();
		}

		public override bool Equals(object obj)
		{
			if (!(obj is LayoutDto entity))
				return false;

			if (Id == entity.Id &&
				VenueId == entity.VenueId &&
				Description.Equals(entity.Description, StringComparison.OrdinalIgnoreCase))
				return true;

			return false;
		}

		public override int GetHashCode() => (Id, VenueId, Description).GetHashCode();
	}
}
