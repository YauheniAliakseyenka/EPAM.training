using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TicketManagementMVC.Infrastructure.Helpers.UserWepiApi;
using TicketManagementMVC.Models;
using TicketManagementMVC.Infrastructure.Extentions;
using Microsoft.IdentityModel.Tokens;

namespace TicketManagementMVC.Infrastructure.Authentication
{
	internal class CustomUserManager
	{
		public const string AccountCultureClaimKey = "AccountCulture";
		private const string BearerTokenClaimKey = "BearerToken";
		private const string RefreshTokenClaimKey = "RefreshToken";

		private const string TokenExpiredHeaderKey = "Token-Expired";

        //identity's creating  after sign in
        public async Task<ResponseModel> CreateIdentity(LoginViewModel userCredentials)
        {
            var content = new StringContent(JsonConvert.SerializeObject(userCredentials), Encoding.UTF8, "application/json");
            var response = await UserWebApiHelper.SendRequest("api/authenticate/token", RequestVerbs.POST, content);

            try
            {
                var getResult = await GetResultFromResponse<TokenModel>(response, "token");

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

			var response = await UserWebApiHelper.SendRequest("api/user/" + identity.GetUserId<int>(),
				RequestVerbs.GET, token: identity.GetClaimValue(BearerTokenClaimKey));

            try
            {
                var getResult = await GetResultFromResponse<User>(response, "user");

                if (getResult.IsSuccess)
                    return getResult;

                // check if an access token expired and repeat request after refreshing of token
                if (getResult.Message.Equals(TokenExpiredHeaderKey, StringComparison.Ordinal))
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
			var response = await UserWebApiHelper.SendRequest("api/authenticate/registration", RequestVerbs.POST, content);

            try
            {
                var getResult = await GetResultFromResponse<TokenModel>(response, "token");

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
            var response = await UserWebApiHelper.SendRequest("api/user/" + identity.GetUserId<int>(),
                RequestVerbs.PUT, content,
                identity.GetClaimValue(BearerTokenClaimKey));

            try
            {
                var getResult = await GetResultFromResponse<string>(response, "message");

                if (getResult.IsSuccess)
                    return getResult;

                // check if an access token expired and repeat request after refreshing of token
                if (getResult.Message.Equals(TokenExpiredHeaderKey, StringComparison.Ordinal))
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

        private async Task RefreshToken(ClaimsIdentity identity)
        {
            var tokens = new TokenModel
            {
                Token = identity.GetClaimValue(BearerTokenClaimKey),
                RefreshToken = identity.GetClaimValue(RefreshTokenClaimKey)
            };
            var content = new StringContent(JsonConvert.SerializeObject(tokens), Encoding.UTF8, "application/json");
            var response = await UserWebApiHelper.SendRequest("api/authenticate/token", RequestVerbs.PUT, content);

            var getResult = await GetResultFromResponse<TokenModel>(response, "token");

            if (!getResult.IsSuccess)
                throw new SecurityTokenException("Token refresh error. Server response:"
                 + JObject.Parse(getResult.Message)["message"]?.ToString());

            var newTokens = getResult.Object as TokenModel;

            if (newTokens is null)
                throw new SecurityTokenException("Invalid token model");

            identity.AddUpdateClaim(BearerTokenClaimKey, newTokens.Token);
            identity.AddUpdateClaim(RefreshTokenClaimKey, newTokens.RefreshToken);
        }

        private async Task<ResponseModel> GetResultFromResponse<T>(HttpResponseMessage response, string sectionKey)
            where T : class
        {
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var obj = JObject.Parse(content)[sectionKey]?.ToString();

                if (obj is null)
                    throw new JsonSerializationException("Section was not found");

                if (typeof(T) == typeof(string))
                    return new ResponseModel
                    {
                        Object = obj,
                        IsSuccess = true
                    };

                var result = JsonConvert.DeserializeObject<T>(obj);

                if (result is null)
                    throw new JsonSerializationException();

                return new ResponseModel
                {
                    Object = result,
                    IsSuccess = true
                };
            }
            else
                 if (response.StatusCode == HttpStatusCode.Unauthorized &&
                response.Headers.Any(x => x.Key.Equals(TokenExpiredHeaderKey, StringComparison.Ordinal)))
                return new ResponseModel
                {
                    Message = TokenExpiredHeaderKey,
                    IsSuccess = false
                };

            var errorMessage = JObject.Parse(content)["message"]?.ToString();

            return new ResponseModel {
                Message = errorMessage ?? "Response error",
                IsSuccess = false };
        }
    }
}