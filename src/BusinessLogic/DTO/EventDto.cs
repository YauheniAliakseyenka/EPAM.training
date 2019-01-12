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
		public string CreatedBy { get; set; }

		public override bool Equals(object obj)
		{
			var entity = obj as EventDto;

			if (entity == null)
				return false;

			if (Id == entity.Id &&
				Title.Equals(entity.Title, StringComparison.OrdinalIgnoreCase) &&
				Description.Equals(entity.Description, StringComparison.OrdinalIgnoreCase) &&
				ImageURL.Equals(entity.ImageURL, StringComparison.Ordinal) &&
				LayoutId == entity.LayoutId &&
				Date.Equals(entity.Date))
				return true;

			return false;
		}

		public override int GetHashCode() => (Id, Title, Description, ImageURL, LayoutId, Date).GetHashCode();
	}
}
