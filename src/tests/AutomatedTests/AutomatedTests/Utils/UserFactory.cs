using System;

namespace AutomatedTests.Utils
{
	public class UserModel
	{
		public string Username { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
	}

	public class UserFactory
	{
		private static string GeneretaUsername() => Guid.NewGuid().ToString();

		private static string GeneretaEmail() => Guid.NewGuid().ToString() + "@gmail.com";

		private static string GeneretaPassword() => GenerateString.Get(15);

        public static UserModel GenerateUser()
        {
            return new UserModel
            {
                Username = GeneretaUsername(),
                Email = GeneretaEmail(),
                Password = GeneretaPassword()
            };
        }

		public static UserModel GetEventManager()
		{
			return new UserModel
			{
				Username = "event_manager",
				Email = string.Empty,
				Password = "1231231231"
			};
		}
	}

	
}
