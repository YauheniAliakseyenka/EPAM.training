using BusinessLogic.DTO;
using System.Threading.Tasks;
using User.WebApi.Models;

namespace User.WebApi.Infrastructure.AuthManager
{
	public interface IAuthManager
	{
		Task<TokenModel> GenerateToken(UserDto user);
        Task<TokenModel> RefreshToken(TokenModel tokenModel);
    }
}
