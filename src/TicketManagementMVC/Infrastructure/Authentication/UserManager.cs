using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TicketManagementMVC.Models;
using TicketManagementMVC.Infrastructure.Extentions;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using User.WebApi.Helper;

namespace TicketManagementMVC.Infrastructure.Authentication
{
	public class CustomUserManager
	{
		public const string AccountCultureClaimKey = "AccountCulture";
		private const string BearerTokenClaimKey = "BearerToken";
		private const string RefreshTokenClaimKey = "RefreshToken";

        private IUserWebApiHelper _requestHelper;

        public CustomUserManager(IUserWebApiHelper requestHelper)
        {
            _requestHelper = requestHelper;
        }

        //identity's creating  after sign in
        public async Task<ResponseModel> CreateIdentity(LoginViewModel userCredentials)
        {
            var content = new StringContent(JsonConvert.SerializeObject(userCredentials), Encoding.UTF8, "application/json");
            var response = await _requestHelper.SendRequest("api/authenticate/token", RequestVerbs.POST, content);

            try
            {
                var getResult = await _requestHelper.GetResultFromResponse<TokenModel>(response, "token");

                if (!getResult.IsSuccess)
                    return getResult;

                var tokens = getResult.Object as TokenModel;

                if (tokens is null)
                    throw new SecurityTokenException("Invalid token model");

                return CreateIdentityFromToken(tokens.Token, tokens.RefreshToken);
            }
            catch (JsonSerializationException exception)
            {
                return new ResponseModel
                {
                    Message = exception.Message,
                    IsSuccess = false
                };
            }
        }

		public async Task<ResponseModel> GetUser(ClaimsIdentity identity)
		{
			if (identity is null)
				throw new ArgumentNullException();

			var response = await _requestHelper.SendRequest("api/user/" + identity.GetUserId<int>(),
				RequestVerbs.GET, token: identity.GetClaimValue(BearerTokenClaimKey));

            try
            {
                var getResult = await _requestHelper.GetResultFromResponse<User>(response, "user");

                if (getResult.IsSuccess)
                    return getResult;

                // check if an access token expired and repeat request after refreshing of token
                if (getResult.Message.Equals(UserWebApiHelper.TokenExpiredHeaderKey, StringComparison.Ordinal))
                {
                    await RefreshToken(identity);

                    return await GetUser(identity);
                }

                // return any other error messages
                return getResult;
            }
            catch (JsonSerializationException exception)
            {
                return new ResponseModel
                {
                    Message = exception.Message,
                    IsSuccess = false
                };
            }
            catch (SecurityTokenException exception)
            {
                return new ResponseModel
                {
                    Message = exception.Message,
                    IsSuccess = false
                };
            }
		}

		//identity's creating after registration
        public async Task<ResponseModel> CreateIdentity(User user)
        {
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
			var response = await _requestHelper.SendRequest("api/authenticate/registration", RequestVerbs.POST, content);

            try
            {
                var getResult = await _requestHelper.GetResultFromResponse<TokenModel>(response, "token");

                if (!getResult.IsSuccess)
                    return getResult;

                var tokens = getResult.Object as TokenModel;

                if (tokens is null)
                    throw new SecurityTokenException("Invalid token model");

                return CreateIdentityFromToken(tokens.Token, tokens.RefreshToken);
            }
            catch (JsonSerializationException exception)
            {
                return new ResponseModel
                {
                    Message = exception.Message,
                    IsSuccess = false
                };
            }
        }

		private ResponseModel CreateIdentityFromToken(string token, string refreshToken)
        {
            //read token
            var tokenDecoder = new JwtSecurityTokenHandler();
            var jwtSecurityToken = (JwtSecurityToken)tokenDecoder.ReadToken(token);

            ClaimsIdentity identity = new ClaimsIdentity("ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            identity.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider",
                "OWIN Provider", ClaimValueTypes.String));

			jwtSecurityToken.Claims.ToList().ForEach(x =>
			{
				identity.AddClaim(x);
			});

            identity.AddClaim(new Claim(BearerTokenClaimKey, token));
			identity.AddClaim(new Claim(RefreshTokenClaimKey, refreshToken));

			return new ResponseModel { Object = identity, IsSuccess = true };
        }

        public async Task<ResponseModel> Update(User user, ClaimsIdentity identity)
        {
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
			var response = await _requestHelper.SendRequest("api/user/" + identity.GetUserId<int>() + "/profile",
				RequestVerbs.PUT, content,
				identity.GetClaimValue(BearerTokenClaimKey));

            try
            {
                var getResult = await _requestHelper.GetResultFromResponse<string>(response, "message");

                if (getResult.IsSuccess)
                    return getResult;

                // check if an access token expired and repeat request after refreshing of token
                if (getResult.Message.Equals(UserWebApiHelper.TokenExpiredHeaderKey, StringComparison.Ordinal))
                {
                    await RefreshToken(identity);

                    return await Update(user, identity);
                }

                // return any other error messages
                return getResult;
            }
            catch (JsonSerializationException exception)
            {
                return new ResponseModel
                {
                    Message = exception.Message,
                    IsSuccess = false
                };
            }
            catch (SecurityTokenException exception)
            {
                return new ResponseModel
                {
                    Message = exception.Message,
                    IsSuccess = false
                };
            }
        }

		public async Task<ResponseModel> UpdateAmount(ClaimsIdentity identity, BalanceUpdateVerb verb, decimal amount = default(decimal))
		{
			HttpResponseMessage response = null;
			ResponseModel result = null;
			User user = null;

			try
			{
				switch (verb)
				{
					case BalanceUpdateVerb.Replenishment:
						result = await GetUser(identity);
						if (result.IsSuccess)
						{
							user = result.Object as User;
							user.Amount += amount;
							var model = new { user.Amount };
							var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
							response = await _requestHelper.SendRequest("api/user/" + identity.GetUserId<int>() + "/balance",
								RequestVerbs.PUT,
								content,
								identity.GetClaimValue(BearerTokenClaimKey));
							result = await _requestHelper.GetResultFromResponse<string>(response, "message");

							if (result.IsSuccess)
								identity.AddUpdateClaim(AuthManager.AccountBalanceClaimKey, user.Amount.ToString(CultureInfo.InvariantCulture));
						}
						break;
					case BalanceUpdateVerb.UpdateFromServer:
						result = await GetUser(identity);
						if (result.IsSuccess)
						{
							user = result.Object as User;
							identity.AddUpdateClaim(AuthManager.AccountBalanceClaimKey, user.Amount.ToString(CultureInfo.InvariantCulture));
						}
						break;
				}
			}
			catch (JsonSerializationException exception)
			{
				return new ResponseModel
				{
					Message = exception.Message,
					IsSuccess = false
				};
			}
			catch (SecurityTokenException exception)
			{
				return new ResponseModel
				{
					Message = exception.Message,
					IsSuccess = false
				};
			}

			return result;
		}

		private async Task RefreshToken(ClaimsIdentity identity)
        {
            var tokens = new TokenModel
            {
                Token = identity.GetClaimValue(BearerTokenClaimKey),
                RefreshToken = identity.GetClaimValue(RefreshTokenClaimKey)
            };
            var content = new StringContent(JsonConvert.SerializeObject(tokens), Encoding.UTF8, "application/json");
            var response = await _requestHelper.SendRequest("api/authenticate/token", RequestVerbs.PUT, content);

            var getResult = await _requestHelper.GetResultFromResponse<TokenModel>(response, "token");

            if (!getResult.IsSuccess)
                throw new SecurityTokenException("Token refresh error. Server response:"
                 + JObject.Parse(getResult.Message)["message"]?.ToString());

            var newTokens = getResult.Object as TokenModel;

            if (newTokens is null)
                throw new SecurityTokenException("Invalid token model");

            identity.AddUpdateClaim(BearerTokenClaimKey, newTokens.Token);
            identity.AddUpdateClaim(RefreshTokenClaimKey, newTokens.RefreshToken);
        }
    }
}