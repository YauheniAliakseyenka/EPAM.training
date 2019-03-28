using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TicketManagementWPF.Infrastructure.Utils.Identity;
using TicketManagementWPF.Models;
using User.WebApi.Helper;

namespace TicketManagementWPF.Infrastructure.Utils.UserManagement
{
	internal class UserManagement : IUserManagement, IUserProfile, IAuth
    {
		private readonly IUserWebApiHelper _userWebApiHelper;
        private readonly IUserIdentity _identity;

        public UserManagement(IUserWebApiHelper userWebApiHelper, IUserIdentity identity)
		{
			this._userWebApiHelper = userWebApiHelper;
            this._identity = identity;
		}
   
		public async Task<ResponseModel> Create(Models.User user)
		{
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            return await SendRequest(content, RequestVerbs.POST, "api/user");
		}
  
		public async Task<ResponseModel> Delete(Models.User user)
		{
            return await SendRequest<string>(RequestVerbs.DELETE, "api/user/" + user.Id, "message");
		}
      
		public async Task<ResponseModel> Update(Models.User user)
		{
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            return await SendRequest(content, RequestVerbs.PUT, "api/user/" + user.Id);
		}

		private async Task<ResponseModel> SendRequest(StringContent content, RequestVerbs verb, string endpoint)
		{
			var token = _identity.GetTokens().Token;
			var response = await _userWebApiHelper.SendRequest(endpoint, verb, content, token);
			var result = await _userWebApiHelper.GetResultFromResponse<string>(response, "message");

			if (!result.IsSuccess && result.Message.Equals(UserWebApiHelper.TokenExpiredHeaderKey, StringComparison.OrdinalIgnoreCase))
			{
				await RefreshToken();
				await SendRequest(content, verb, endpoint);
			}

			return result;
		}

		private async Task<ResponseModel> SendRequest<T>(RequestVerbs verb, string endpoint) where T :class
		{
			var token = _identity.GetTokens().Token;
			var response = await _userWebApiHelper.SendRequest(endpoint, verb, token: token);
			var result = await _userWebApiHelper.GetResultFromResponse<T>(response);

			if (!result.IsSuccess && result.Message.Equals(UserWebApiHelper.TokenExpiredHeaderKey, StringComparison.OrdinalIgnoreCase))
			{
				await RefreshToken();
				await SendRequest<T>(verb, endpoint);
			}

			return result;
		}

		private async Task<ResponseModel> SendRequest<T>(RequestVerbs verb, string endpoint, string section) where T : class
		{
			var token = _identity.GetTokens().Token;
			var response = await _userWebApiHelper.SendRequest(endpoint, verb, token: token);
			var result = await _userWebApiHelper.GetResultFromResponse<T>(response, section);

			if (!result.IsSuccess && result.Message.Equals(UserWebApiHelper.TokenExpiredHeaderKey, StringComparison.OrdinalIgnoreCase))
			{
				await RefreshToken();
				await SendRequest<T>(verb, endpoint);
			}

			return result;
		}

        private async Task<ResponseModel> SendRequest<T>(
            StringContent content, 
            RequestVerbs verb, 
            string endpoint, 
            string section) where T:class
        {
            var token = _identity.GetTokens().Token;
            var response = await _userWebApiHelper.SendRequest(endpoint, verb, content, token);
            var result = await _userWebApiHelper.GetResultFromResponse<T>(response, section);

            if (!result.IsSuccess && result.Message.Equals(UserWebApiHelper.TokenExpiredHeaderKey, StringComparison.OrdinalIgnoreCase))
            {
                await RefreshToken();
                await SendRequest<T>(content, verb, endpoint, section);
            }

            return result;
        }

        private async Task RefreshToken()
		{
            var tokens = _identity.GetTokens();
            var content = new StringContent(JsonConvert.SerializeObject(tokens), Encoding.UTF8, "application/json");
			var response = await _userWebApiHelper.SendRequest("api/authenticate/token", RequestVerbs.PUT, content);
			var result = await _userWebApiHelper.GetResultFromResponse<TokenModel>(response, "token");

			if (!result.IsSuccess)
				throw new InvalidOperationException("Update token error");

			var newTokens = result.Object as TokenModel;
            _identity.SaveTokens(newTokens);
		}

		public async Task<ResponseModel> ToList()
		{
			return await SendRequest<UserList>( RequestVerbs.GET, "api/user");
		}

	    public async Task<ResponseModel> Get(int id)
		{
			return await SendRequest<Models.User>(RequestVerbs.GET, "api/user/" + id, "user");
		}
      
        async Task<ResponseModel> IUserProfile.Update(Models.User user)
        {
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            return await SendRequest(content, RequestVerbs.PUT, "api/user/" + user.Id + "/profile");
        }
    
        public async Task<ResponseModel> Get()
        {
            return await SendRequest<Models.User>(RequestVerbs.GET, "api/user/" + _identity.GetId(), "user");
        }

        public async Task<ResponseModel> SignIn(AuthModel credentials)
        {
            var content = new StringContent(JsonConvert.SerializeObject(credentials), Encoding.UTF8, "application/json");
            return await SendRequest<TokenModel>(content, RequestVerbs.POST, "api/authenticate/token", "token");
        }

		public async Task<ResponseModel> FindBy(UserSearchEnum by, string value)
		{
			var enpoint = string.Empty;

			switch(by)
			{
				case UserSearchEnum.Email:
					enpoint = "api/user/search/email/" + value;
					break;
				case UserSearchEnum.Username:
					enpoint = "api/user/search/username/" + value;
					break;
			}

			return await SendRequest<Models.User>(RequestVerbs.GET, enpoint, "user");
		}
	}
}
