using System.ComponentModel;

namespace BusinessLogic.Services
{
	public enum Role
	{
		[Description("User")]
		User,
		[Description("Venue manager")]
		VenueManager,
		[Description("Event manager")]
		EventManager
	}
}
