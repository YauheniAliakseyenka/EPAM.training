using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using TicketManagementWPF.Helpers;
using TicketManagementWPF.Models;

namespace TicketManagementWPF.Infrastructure.Utils.Identity
{
    internal class UserIdentity : IUserIdentity
    {
        public int GetId()
        {
			var token = ReadToken();

			return int.Parse(token.Claims.
                SingleOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier, System.StringComparison.Ordinal)).
                Value);
        }

        public TokenModel GetTokens()
        {
            return new TokenModel
            {
                Token = Properties.Settings.Default.AccessToken,
                RefreshToken = Properties.Settings.Default.RefreshToken
            };
        }

		public bool IsAdmin()
		{
			var token = ReadToken();
			var adminRoleStr = EnumHelper.GetDescription(Role.VenueManager);

			return token.Claims.Any(x => x.Type.Equals(ClaimTypes.Role, System.StringComparison.Ordinal) &&
									x.Value.Equals(adminRoleStr, System.StringComparison.OrdinalIgnoreCase));
		}

		public void SaveTokens(TokenModel tokens)
        {
            Properties.Settings.Default.AccessToken = tokens.Token;
            Properties.Settings.Default.RefreshToken = tokens.RefreshToken;
            Properties.Settings.Default.Save();
        }

		private JwtSecurityToken ReadToken()
		{
			var tokenDecoder = new JwtSecurityTokenHandler();

			return (JwtSecurityToken)tokenDecoder.ReadToken(GetTokens().Token);
		}
    }
}
