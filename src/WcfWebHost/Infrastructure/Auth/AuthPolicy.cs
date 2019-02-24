using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;

namespace WcfWebHost.Infrastructure.Auth
{
    internal class AuthPolicy : IAuthorizationPolicy
    {
        public ClaimSet Issuer => ClaimSet.System;
        public string Id => Guid.NewGuid().ToString();         

        public bool Evaluate(EvaluationContext evaluationContext, ref object state)
        {
            var claimsPrincipal = new ClaimsPrincipal(GetIdentity(evaluationContext));
            evaluationContext.Properties["Principal"] = claimsPrincipal;
            Thread.CurrentPrincipal = claimsPrincipal;

            return true;
        }

        private ClaimsIdentity GetIdentity(EvaluationContext evaluationContext)
        {
			if (evaluationContext.Properties.TryGetValue("Identities", out var identities))
			{
				var identity = (identities as List<IIdentity>)?.Single();
				var claimsIdentity = new ClaimsIdentity(identity);

				var roles = AuthSettings.Settings.Clients[identity.Name].Roles;

				if (roles != null)
				{
					foreach (var role in roles)
						claimsIdentity.AddClaim(
							new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, (role as RoleElement).Name));
				}

				return claimsIdentity;
			}

			return new ClaimsIdentity();
		}
    }
}