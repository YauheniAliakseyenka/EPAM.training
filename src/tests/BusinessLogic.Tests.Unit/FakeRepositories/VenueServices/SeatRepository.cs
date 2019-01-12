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
	internal class SeatRepository: IRepository<Seat, int>
	{
		private List<Seat> _list;

		public SeatRepository()
		{
			_list = new List<Seat>
			{
				new Seat { Id = 1, AreaId = 1, Number = 1, Row = 1},
				new Seat { Id = 2, AreaId = 1, Number = 2, Row = 1},
				new Seat { Id = 3, AreaId = 1, Number = 3, Row = 1},
				new Seat { Id = 4, AreaId = 1, Number = 1, Row = 2},
				new Seat { Id = 5, AreaId = 1, Number = 2, Row = 2},
				new Seat { Id = 6, AreaId = 2, Number = 1, Row = 1},
				new Seat { Id = 7, AreaId = 2, Number = 2, Row = 1},
				new Seat { Id = 8, AreaId = 2, Number = 1, Row = 2},
				new Seat { Id = 9, AreaId = 7, Number = 1, Row = 1},
				new Seat { Id = 15, AreaId = 4, Number = 1, Row = 1},
				new Seat { Id = 10, AreaId = 7, Number = 1, Row = 2},
				new Seat { Id = 11, AreaId = 7, Number = 2, Row = 1},
				new Seat { Id = 12, AreaId = 7, Number = 2, Row = 2},
				new Seat { Id = 13, AreaId = 3, Number = 1, Row = 1},
				new Seat { Id = 14, AreaId = 5, Number = 1, Row = 1},
				new Seat { Id = 17, AreaId = 6, Number = 1, Row = 1},
				new Seat { Id = 16, AreaId = 8, Number = 1, Row = 1}
			};
		}

		public void Create(Seat entity)
		{
			_list.Add(entity);
		}

		public void Delete(int id)
		{
			_list.Remove(_list.Find(x => x.Id == id));
		}

		public void DeleteBy(Expression<Func<Seat, bool>> expression)
		{
			_list.RemoveAll(new Predicate<Seat>(expression.Compile()));
		}

		public IQueryable<Seat> FindBy(Expression<Func<Seat, bool>> expression)
		{
			return _list.FindAll(new Predicate<Seat>(expression.Compile())).AsQueryable();
		}

		public Seat Get(int id)
		{
			return _list.FirstOrDefault(x => x.Id == id);
		}

		public IQueryable<Seat> GetList()
		{
			return _list.ToList().AsQueryable();
		}

		public void Update(Seat entity)
		{
			var update = _list.FirstOrDefault(x => x.Id == entity.Id);
			update.Row = entity.Row;
			update.Number = entity.Number;
			update.AreaId = entity.AreaId;
		}

        public Task<IEnumerable<Seat>> FindByAsync(Expression<Func<Seat, bool>> expression)
        {
            return Task.FromResult(_list.FindAll(new Predicate<Seat>(expression.Compile())).AsEnumerable());
        }

        public Task<Seat> GetAsync(int id)
        {
            return Task.FromResult(_list.FirstOrDefault(x => x.Id == id));
        }

        public Task<IEnumerable<Seat>> GetListAsync()
        {
            return Task.FromResult(_list.AsEnumerable());
        }
    }
}
