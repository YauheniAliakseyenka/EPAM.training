using BusinessLogic.DTO;
using System.Collections.Generic;

namespace BusinessLogic.BusinessModels
{
	public class EventModel
	{
		public VenueDto Venue { get; set; }
		public string LayoutName { get; set; }
		public List<EventAreaDto> Areas { get; set; }
		public EventDto Event { get; set; }
		public bool IsPublished { get; set; }
	}
}
