using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace User.WebApi.Helper
{
    public class UserWebApiHelper : IUserWebApiHelper
	{
        public const string TokenExpiredHeaderKey = "Token-Expired";

        private readonly string _uri;

        public UserWebApiHelper(string defaultUri)
        {
            _uri = defaultUri;
        }

        public async Task<HttpResponseMessage> SendRequest(string endpoint, 
            RequestVerbs verb, 
            StringContent bodyContent = null,
            string token = null)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_uri);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                if(!string.IsNullOrEmpty(token))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage result = null;
                switch (verb)
                {
                    case RequestVerbs.PUT:
                        if (bodyContent is null)
                            throw new ArgumentNullException();
							result = await client.PutAsync(endpoint, bodyContent);
						break;

                    case RequestVerbs.POST:
                        if (bodyContent is null)
                            throw new ArgumentNullException();
							result = await client.PostAsync(endpoint, bodyContent);
						break;

                    case RequestVerbs.GET:
						result = await client.GetAsync(endpoint);
                        break;

                    case RequestVerbs.DELETE:
                        result = await client.DeleteAsync(endpoint);
                        break;
                }

                return result;
            }
        }

		public async Task<ResponseModel> GetResultFromResponse<T>(HttpResponseMessage response, string sectionKey)
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

			return new ResponseModel
			{
				Message = errorMessage ?? "Response error",
				IsSuccess = false
			};
		}

		public async Task<ResponseModel> GetResultFromResponse<T>(HttpResponseMessage response)
			where T : class
		{
			var content = await response.Content.ReadAsStringAsync();

			if (response.IsSuccessStatusCode)
			{
				var result = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());

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

			return new ResponseModel
			{
				Message = errorMessage ?? "Response error",
				IsSuccess = false
			};
		}
	}
}