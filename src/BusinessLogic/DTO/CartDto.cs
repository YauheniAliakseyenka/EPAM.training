using System;

namespace BusinessLogic.DTO
{
	public class CartDto
	{
		public int Id { get; set; }
		public string UserId { get; set; }

		public override bool Equals(object obj)
		{
			var entity = obj as CartDto;

			if (entity == null)
				return false;

			if (Id == entity.Id && UserId.Equals(entity.UserId, StringComparison.Ordinal))
				return true;

			return false;
		}

		public override int GetHashCode() => (Id, UserId).GetHashCode();
	}
}
