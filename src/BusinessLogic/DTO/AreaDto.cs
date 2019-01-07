using System;
using System.Collections.Generic;
using System.Linq;

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
			var entity = obj as AreaDto;

			if (entity == null)
				return false;

			if (Id == entity.Id &&
				LayoutId == entity.LayoutId &&
				Description.Equals(entity.Description, StringComparison.OrdinalIgnoreCase) &&
				CoordX == entity.CoordX &&
				CoordY == entity.CoordY &&
				SeatList.SequenceEqual(entity.SeatList))
				return true;

			return false;
		}

		public override int GetHashCode() => (Id, LayoutId, Description, CoordX, CoordY).GetHashCode();
	}
}
