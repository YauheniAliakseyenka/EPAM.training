using Microsoft.AspNet.Identity;

namespace TicketManagementMVC.Infrastructure.Authentication
{
	public class ApplicationUserManager: UserManager<User, int>
	{
		public ApplicationUserManager(IUserStore<User, int> store)
			: base(store)
		{
			configurate(this);
		}

		private static void configurate(UserManager<User, int> manager)
		{
			manager.UserValidator = new UserValidator<User, int>(manager)
			{
				AllowOnlyAlphanumericUserNames = false,
				RequireUniqueEmail = true
			};
		}
	}
}