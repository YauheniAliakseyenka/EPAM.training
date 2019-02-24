using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;

namespace WcfWebHost.Infrastructure.Auth
{
    internal class UserValidator : UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            var user = AuthSettings.Settings.Clients[userName];

            if (user is null || !user.Password.Equals(password, StringComparison.Ordinal))
				throw new SecurityTokenValidationException();

			return;
        }
    }
}