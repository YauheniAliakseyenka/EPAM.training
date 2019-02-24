using TicketManagementMVC.Models;
using TicketManagementMVC.Infrastructure.Authentication;

namespace TicketManagementMVC.Infrastructure.Helpers.Parsers
{
	internal static class UserParser
	{
		public static User RegistrationViewModelToUser(RegistrationViewModel from)
		{
			return new User
			{
				UserName = from.UserName,
				Timezone = from.SelectedTimezone,
				Surname = from.Surname,
				Culture = from.Culture,
				Email = from.Email,
				Firstname = from.Firstname,
				Password = from.Password
			};
		}

		public static User UserProfileViewModelToUser(UserProfileViewModel from)
		{
			return new User
			{
				Surname = from.Surname,
				Culture = from.Culture,
				Email = from.Email,
				Firstname = from.Firstname,
				Timezone = from.Timezone
			};
		}
	}
}