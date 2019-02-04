using System;

namespace BusinessLogic.DTO
{
	public class CartDto
	{
		public int Id { get; set; }
		public int UserId { get; set; }

		public override bool Equals(object obj)
		{
			if (!(obj is CartDto entity))
				return false;

			if (Id == entity.Id && UserId == entity.UserId)
				return true;

			return false;
		}

		public override int GetHashCode() => (Id, UserId).GetHashCode();
	}
}
