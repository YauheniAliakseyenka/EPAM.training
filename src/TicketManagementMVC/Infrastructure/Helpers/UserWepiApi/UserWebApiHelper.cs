using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TicketManagementMVC.Infrastructure.Helpers.UserWepiApi
{
    internal static class UserWebApiHelper
    {
        public static async Task<HttpResponseMessage> SendRequest(string endpoint, 
            RequestVerbs verb, 
            StringContent bodyContent = null,
            string token = null)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["UserWebApiBaseAddress"]);
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
	}
}