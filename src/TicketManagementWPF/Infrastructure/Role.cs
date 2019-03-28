using System.ComponentModel;

namespace TicketManagementWPF.Infrastructure
{
	internal enum Role
	{
		[Description("User")]
		User,
		[Description("Venue manager")]
		VenueManager,
		[Description("Event manager")]
		EventManager
	}
}
