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
	}
}
