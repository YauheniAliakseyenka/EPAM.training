using BusinessLogic.Services;
using System;

namespace BusinessLogic.DTO
{
	public class EventSeatDto : IComparable<EventSeatDto>
	{
		public int Id { get; set; }
		public int EventAreaId { get; set; }
		public int Row { get; set; }
		public int Number { get; set; }
		public SeatState State { get; set; }

		public int CompareTo(EventSeatDto other)
		{
			if (Row > other.Row)
				return 1;
			else
					if (Row == other.Row)
			{
				if (Number > other.Number)
					return 1;
				if (Number < other.Number)
					return -1;

				return 0;
			}
			else
					if (Row < other.Row)
				return -1;

			return 0;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is EventSeatDto entity))
				return false;

			if (Id == entity.Id &&
				EventAreaId == entity.EventAreaId &&
				Row == entity.Row &&
				Number == entity.Number &&
				State == entity.State)
				return true;

			return false;
		}

		public override int GetHashCode() => (Id, EventAreaId, Row, Number, State).GetHashCode();
	}
}
