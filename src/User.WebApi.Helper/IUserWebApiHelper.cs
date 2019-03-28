using System.Net.Http;
using System.Threading.Tasks;

namespace User.WebApi.Helper
{
	public interface IUserWebApiHelper
	{
		Task<HttpResponseMessage> SendRequest(string endpoint,
			RequestVerbs verb,
			StringContent bodyContent = null,
			string token = null);
		Task<ResponseModel> GetResultFromResponse<T>(HttpResponseMessage response, string sectionKey) where T : class;
		Task<ResponseModel> GetResultFromResponse<T>(HttpResponseMessage response) where T : class;
	}
}
