using BusinessLogic.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
	public interface IUserService :IStoreService<UserDto, string>
	{
		Task<IEnumerable<string>> GetRoles(string userName);
	}
}
