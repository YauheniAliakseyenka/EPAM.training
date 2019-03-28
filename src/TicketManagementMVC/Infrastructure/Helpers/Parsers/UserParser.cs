using TicketManagementMVC.Models;

namespace TicketManagementMVC.Infrastructure.Helpers.Parsers
{
	internal static class UserParser
	{
		public static Authentication.User RegistrationViewModelToUser(RegistrationViewModel from)
		{
			return new Authentication.User
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

		public static Authentication.User UserProfileViewModelToUser(UserProfileViewModel from)
		{
			return new Authentication.User
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