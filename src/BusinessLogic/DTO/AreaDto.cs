using System;
using System.Collections.Generic;

namespace BusinessLogic.DTO
{
	public class AreaDto
	{
		public int Id { get; set; }
		public int LayoutId { get; set; }
		public string Description { get; set; }
		public int CoordX { get; set; }
		public int CoordY { get; set; }
		public List<SeatDto> SeatList { get; set; }

		public AreaDto()
		{
			SeatList = new List<SeatDto>();
		}

		public override bool Equals(object obj)
		{
			if (!(obj is AreaDto entity))
				return false;

			if (Id == entity.Id &&
				LayoutId == entity.LayoutId &&
				Description.Equals(entity.Description, StringComparison.OrdinalIgnoreCase) &&
				CoordX == entity.CoordX &&
				CoordY == entity.CoordY)
				return true;

			return false;
		}

		public override int GetHashCode() => (Id, LayoutId, Description, CoordX, CoordY).GetHashCode();
	}
}
