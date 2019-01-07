using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services.UserServices
{
	internal partial class UserService
	{
		public IEnumerable<string> GetRoles(string userName)
		{
			var roles = (from user in _context.UserRepository.GetList()
						 join userRole in _context.UserRoleRepository.GetList() on user.Id equals userRole.UserId
						 join role in _context.RoleRepository.GetList() on userRole.RoleId equals role.Id
						 where user.UserName == userName
						 select role.Name).ToList();

			return roles;
		}
	}
}
