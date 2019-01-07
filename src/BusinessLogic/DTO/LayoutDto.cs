using System;
using System.Collections.Generic;
using System.Linq;

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
			var entity = obj as LayoutDto;

			if (entity == null)
				return false;

			if (Id == entity.Id &&
				VenueId == entity.VenueId &&
				Description.Equals(entity.Description, StringComparison.OrdinalIgnoreCase) &&
				AreaList.SequenceEqual(entity.AreaList))
				return true;

			return false;
		}

		public override int GetHashCode() => (Id, VenueId, Description).GetHashCode();
	}
}
