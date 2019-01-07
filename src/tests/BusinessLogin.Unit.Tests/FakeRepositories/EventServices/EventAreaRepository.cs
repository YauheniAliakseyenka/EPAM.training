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
	internal class EventAreaRepository : IRepository<EventArea, int>
	{
		private List<EventArea> _list;

		public EventAreaRepository()
		{
			_list = new List<EventArea>
			{
				new EventArea{ Id = 1, EventId = 1, Description = "The area #1", CoordX = 1, CoordY = 1,
					AreaDefaultId = 1, Price = 145.25M },
					new EventArea{ Id = 2, EventId = 1, Description = "The area #2", CoordX = 1, CoordY = 2,
						AreaDefaultId = 3, Price = 45.7M},
					new EventArea{ Id = 3, EventId = 1, Description = "The area #3", CoordX = 1, CoordY = 1,
						AreaDefaultId = 5, Price = 180.25M},
					new EventArea{ Id = 9, EventId = 3, Description = "The first area", CoordX = 1, CoordY = 1,
						AreaDefaultId = 7, Price = 165.96M},
			};
		}

		public void Create(EventArea entity)
		{
			_list.Add(entity);
		}

		public void Delete(int id)
		{
			_list.Remove(_list.Find(x => x.Id == id));
		}

		public void DeleteBy(Expression<Func<EventArea, bool>> expression)
		{
			_list.RemoveAll(new Predicate<EventArea>(expression.Compile()));
		}

		public IQueryable<EventArea> FindBy(Expression<Func<EventArea, bool>> expression)
		{
			return _list.FindAll(new Predicate<EventArea>(expression.Compile())).AsQueryable();
		}

		public EventArea Get(int id)
		{
			return _list.FirstOrDefault(x => x.Id == id);
		}

		public IQueryable<EventArea> GetList()
		{
			return _list.ToList().AsQueryable();
		}

		public void Update(EventArea entity)
		{
			var update = _list.FirstOrDefault(x => x.Id == entity.Id);
			update.Description = entity.Description;
			update.CoordX = entity.CoordX;
			update.CoordY = entity.CoordY;
			update.EventId = entity.EventId;
			update.Price = entity.Price;
			update.AreaDefaultId = entity.AreaDefaultId;
		}
	}
}
