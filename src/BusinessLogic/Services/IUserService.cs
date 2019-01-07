using BusinessLogic.DTO;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
	public interface IUserService :IStoreService<UserDto, string>
	{
		IEnumerable<string> GetRoles(string userName);
	}
}
