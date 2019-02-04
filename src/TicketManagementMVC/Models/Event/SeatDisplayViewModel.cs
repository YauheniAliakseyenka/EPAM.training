using System;

namespace TicketManagementMVC.Models
{
	public class SeatDisplayViewModel
	{
		public int SeatId { get; set; }
		public string EventName { get; set; }
		public DateTime Date { get; set; }
        public decimal Price { get; set; }
		public string Venue { get; set; }
		public string Layout { get; set; }
		public int SeatRow { get; set; }
        public int SeatNumber { get; set; }
		public string Area { get; set; }
	}
}