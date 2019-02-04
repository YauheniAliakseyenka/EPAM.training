using System;

namespace BusinessLogic.DTO
{
	public class OrderDto
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public DateTime Date { get; set; }

        public override bool Equals(object obj)
        {
			if (!(obj is OrderDto entity))
				return false;

			if (Id == entity.Id &&
                UserId == entity.UserId &&
				Date.Equals(entity.Date))
                return true;

            return false;
        }

        public override int GetHashCode() => (Id, UserId, Date).GetHashCode();
    }
}
