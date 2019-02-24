using Microsoft.Owin.Security;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Web;
using TicketManagementMVC.Infrastructure.Extentions;

namespace TicketManagementMVC.Infrastructure.Authentication
{
	internal class AuthManager
	{
		public const string AccountBalanceClaimKey = "AccountBalance";

		private readonly HttpContextBase _httpContext;

		public AuthManager(HttpContextBase httpContext)
		{
			this._httpContext = httpContext;
		}

		public void SignIn(ClaimsIdentity identity, out string culture)
		{
            _httpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties
            {
                IsPersistent = true
            }, identity);

            //get current user culture value, which stored in db
            culture = identity.Claims.FirstOrDefault(x => x.Type.Equals("Culture", System.StringComparison.Ordinal))?.Value;
		}

		public void SignIn(ClaimsIdentity identity)
		{
			_httpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties
			{
				IsPersistent = true
			}, identity);
		}

		public void SetAccountBalance(string amount)
		{
			if (_httpContext.User.Identity.IsAuthenticated)
				_httpContext.User.AddUpdateClaim(AccountBalanceClaimKey, amount);
		}

		public decimal GetAccountBalance()
		{
			if (!decimal.TryParse(_httpContext.User.GetClaimValue(AccountBalanceClaimKey),
				NumberStyles.Currency, CultureInfo.InvariantCulture, out var amount))
				throw new InvalidCastException();

			return amount;
		} 

		public void SignOut(params string[] authenticationTypes) => _httpContext.GetOwinContext().Authentication.SignOut(authenticationTypes);
	}
}