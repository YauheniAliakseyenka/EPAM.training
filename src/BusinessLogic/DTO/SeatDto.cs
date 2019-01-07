namespace BusinessLogic.DTO
{
	public class SeatDto
	{
		public int Id { get; set; }
		public int AreaId { get; set; }
		public int Row { get; set; }
		public int Number { get; set; }

		public override bool Equals(object obj)
		{
			var entity = obj as SeatDto;

			if (entity == null)
				return false;

			if (Id == entity.Id &&
				AreaId == entity.AreaId &&
				Row == entity.Row &&
				Number == entity.Number)
				return true;

			return false;
		}

		public override int GetHashCode() => (Id, AreaId, Row, Number).GetHashCode();
	}
}
