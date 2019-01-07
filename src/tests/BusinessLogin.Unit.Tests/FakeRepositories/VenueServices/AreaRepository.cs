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
	internal class AreaRepository : IRepository<Area, int>
	{
		private List<Area> _list;

		public AreaRepository()
		{
			_list = new List<Area>
			{
				new Area{ Id = 1, LayoutId = 1, Description = "The area #1", CoordX = 1, CoordY = 1},
				new Area{ Id = 2, LayoutId = 3, Description = "Cheap area", CoordX = 2, CoordY = 2},
				new Area{ Id = 3, LayoutId = 1, Description = "The area #2", CoordX = 1, CoordY = 2},
				new Area{ Id = 4, LayoutId = 3, Description = "Expensive area", CoordX = 3, CoordY = 3},
				new Area{ Id = 5, LayoutId = 1, Description = "The area #3", CoordX = 1, CoordY = 1},
				new Area{ Id = 6, LayoutId = 4, Description = "The first area", CoordX = 3, CoordY = 3},
				new Area{ Id = 7, LayoutId = 5, Description = "The first area", CoordX = 1, CoordY = 1},
				new Area{ Id = 8, LayoutId = 6, Description = "The area #1", CoordX = 1, CoordY = 1},
				new Area{ Id = 9, LayoutId = 6, Description = "The area #2", CoordX = 1, CoordY = 1}
			};
		}

		public void Create(Area entity)
		{
			_list.Add(entity);
		}

		public void Delete(int id)
		{
			_list.Remove(_list.Find(x => x.Id == id));
		}

		public void DeleteBy(Expression<Func<Area, bool>> expression)
		{
			_list.RemoveAll(new Predicate<Area>(expression.Compile()));
		}

		public IQueryable<Area> FindBy(Expression<Func<Area, bool>> expression)
		{
			return _list.FindAll(new Predicate<Area>(expression.Compile())).AsQueryable();
		}

		public Area Get(int id)
		{
			return _list.FirstOrDefault(x => x.Id == id);
		}

		public IQueryable<Area> GetList()
		{
			return _list.ToList().AsQueryable();
		}

		public void Update(Area entity)
		{
			var update = _list.FirstOrDefault(x => x.Id == entity.Id);
			update.Description = entity.Description;
			update.CoordX = entity.CoordX;
			update.CoordY = entity.CoordY;
			update.LayoutId = entity.LayoutId;
		}
	}
}
