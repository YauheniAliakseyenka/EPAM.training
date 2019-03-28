using BusinessLogic.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
	public interface IUserService :IStoreService<UserDto, int>
	{
		Task<IEnumerable<string>> GetRoles(string userName);
		Task<UserDto> FindByEmail(string email);
		Task<UserDto> FindByUserName(string userName);
		Task<UserDto> FindById(int id);
        Task<string> GetRefreshToken(int userId);
        Task SetRefreshToken(int userId, string token);
		Task AddRole(UserDto user,Role role);
		Task DeleteRole(UserDto user, Role role);
    }
}
