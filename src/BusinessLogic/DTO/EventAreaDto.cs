using System;
using System.Collections.Generic;

namespace BusinessLogic.DTO
{
	public class EventAreaDto : IComparable<EventAreaDto>
	{
		public int Id { get; set; }
		public int EventId { get; set; }
		public string Description { get; set; }
		public int CoordX { get; set; }
		public int CoordY { get; set; }
		public decimal Price { get; set; }
		public int AreaDefaultId { get; set; }		
		public List<EventSeatDto> Seats { get; set; }

		public EventAreaDto()
		{
			Seats = new List<EventSeatDto>();
		}

		public override bool Equals(object obj)
		{
			var entity = obj as EventAreaDto;

			if (entity == null)
				return false;

			if (Id == entity.Id &&
				EventId == entity.EventId &&
				Description.Equals(entity.Description, StringComparison.OrdinalIgnoreCase) &&
				CoordX == entity.CoordX &&
				CoordY == entity.CoordY &&
				Price == entity.Price &&
				AreaDefaultId == entity.AreaDefaultId)
				return true;

			return false;
		}

		public override int GetHashCode() => (Id, EventId, Description, CoordX, CoordY, Price, AreaDefaultId).GetHashCode();		

		public int CompareTo(EventAreaDto other)
		{
			if (CoordY > other.CoordY)
				return 1;
			else
				if (CoordY == other.CoordY)
			{
				if (CoordX > other.CoordX)
					return 1;
				if (CoordX < CoordX)
					return -1;

				return 0;
			}
			else
				if (CoordY < other.CoordY)
				return -1;

			return 0;
		}
	}
}
