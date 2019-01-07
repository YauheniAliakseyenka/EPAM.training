using DataAccess.Entities;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogin.Unit.Tests.FakeRepositories
{
	internal class UserRoleRepository : IRepository<UserRole, int>
	{
		private List<UserRole> _list;

		public UserRoleRepository()
		{
			_list = new List<UserRole>
			{
				new UserRole{ RoleId = 2, UserId = "57b9433e-78ac-4619-91eb-dd7a5c130f08"},
				new UserRole{ RoleId = 1, UserId = "57b9433e-78ac-4619-91eb-dd7a5c130f08"},
				new UserRole{ RoleId = 1, UserId = "c4195241-4621-4e52-95a1-714d2f005cbb"}
			};
		}

		public void Create(UserRole entity)
		{
			_list.Add(entity);
		}

		public void Delete(int id)
		{
			throw new NotImplementedException();
		}

		public void DeleteBy(Expression<Func<UserRole, bool>> expression)
		{
			_list.RemoveAll(new Predicate<UserRole>(expression.Compile()));
		}

		public IQueryable<UserRole> FindBy(Expression<Func<UserRole, bool>> expression)
		{
			return _list.FindAll(new Predicate<UserRole>(expression.Compile())).AsQueryable();
		}

		public UserRole Get(int id)
		{
			throw new NotImplementedException();
		}

		public IQueryable<UserRole> GetList()
		{
			return _list.ToList().AsQueryable();
		}

		public void Update(UserRole entity)
		{
			throw new NotImplementedException();
		}
	}
}
