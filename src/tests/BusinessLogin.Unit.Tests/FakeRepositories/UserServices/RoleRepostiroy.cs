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
	internal class RoleRepostiroy : IRepository<Role, int>
	{
		private List<Role> _list;

		public RoleRepostiroy()
		{
			_list = new List<Role>
			{
				new Role{ Id =1, Name = "User"},
				new Role{ Id =2, Name = "Venue manager"},
				new Role{ Id =3, Name = "Event manager"}
			};
		}

		public void Create(Role entity)
		{
			_list.Add(entity);
		}

		public void Delete(int id)
		{
			_list.Remove(_list.Find(x => x.Id == id));
		}

		public void DeleteBy(Expression<Func<Role, bool>> expression)
		{
			_list.RemoveAll(new Predicate<Role>(expression.Compile()));
		}

		public IQueryable<Role> FindBy(Expression<Func<Role, bool>> expression)
		{
			return _list.FindAll(new Predicate<Role>(expression.Compile())).AsQueryable();
		}

		public Role Get(int id)
		{
			return _list.FirstOrDefault(x => x.Id == id);
		}

		public IQueryable<Role> GetList()
		{
			return _list.ToList().AsQueryable();
		}

		public void Update(Role entity)
		{
			var update = _list.FirstOrDefault(x => x.Id == entity.Id);
			update.Name = entity.Name;
		}
	}
}
