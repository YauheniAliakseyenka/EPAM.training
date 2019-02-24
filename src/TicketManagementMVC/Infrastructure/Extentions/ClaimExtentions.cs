using Microsoft.Owin.Security;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace TicketManagementMVC.Infrastructure.Extentions
{
	public static class ClaimExtentions
	{
		public static void AddUpdateClaim(this IPrincipal currentPrincipal, string key, string value)
		{
			var identity = currentPrincipal.Identity as ClaimsIdentity;
            AddUpdateClaim(identity, key, value);
		}

		public static string GetClaimValue(this IPrincipal currentPrincipal, string key)
		{
			var identity = currentPrincipal.Identity as ClaimsIdentity;

            return GetClaimValue(identity, key);
		}

		public static void AddUpdateClaim(this ClaimsIdentity identity, string key, string value)
		{
			if (identity == null)
				return;

			// check for existing claim and remove it
			var existingClaim = identity.FindFirst(key);
			if (existingClaim != null)
				identity.RemoveClaim(existingClaim);

			// add new claim
			identity.AddClaim(new Claim(key, value));
			var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
			authenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(identity), new AuthenticationProperties() { IsPersistent = true });
		}

		public static string GetClaimValue(this ClaimsIdentity identity, string key)
		{
			if (identity == null)
				return null;

			var claim = identity.Claims.FirstOrDefault(c => c.Type.Equals(key, System.StringComparison.Ordinal));

			return claim?.Value;
		}
	}
}