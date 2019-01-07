using BusinessLogic.DTO;

namespace BusinessLogic.BusinessModels
{
    public class SeatModel
    {
        public EventDto Event { get; set; }
        public EventSeatDto Seat { get; set; }
        public EventAreaDto Area { get; set; }
		public LayoutDto Layout { get; set; }
		public VenueDto Venue { get; set; }
	}
}
