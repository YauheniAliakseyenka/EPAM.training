using System;

namespace BusinessLogic.DTO
{
	public class EventDto
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string ImageURL { get; set; }
		public int LayoutId { get; set; }
		public DateTime Date { get; set; }
		public int CreatedBy { get; set; }

		public override bool Equals(object obj)
		{
			if (!(obj is EventDto entity))
				return false;

			if (Id == entity.Id &&
				Title.Equals(entity.Title, StringComparison.OrdinalIgnoreCase) &&
				Description.Equals(entity.Description, StringComparison.OrdinalIgnoreCase) &&
				ImageURL.Equals(entity.ImageURL, StringComparison.Ordinal) &&
				LayoutId == entity.LayoutId &&
				Date.Equals(entity.Date) &&
				CreatedBy == entity.CreatedBy)
				return true;

			return false;
		}

		public override int GetHashCode() => (Id, Title, Description, ImageURL, LayoutId, Date, CreatedBy).GetHashCode();
	}
}
