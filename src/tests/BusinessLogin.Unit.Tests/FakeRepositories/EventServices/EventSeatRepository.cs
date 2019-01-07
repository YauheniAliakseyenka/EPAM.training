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
	internal class EventSeatRepository : IRepository<EventSeat, int>
	{
		private List<EventSeat> _list;

		public EventSeatRepository()
		{
			_list = new List<EventSeat>
			{
				new EventSeat { Id = 1, EventAreaId = 1, Number = 1, Row = 1, State = 0},
				new EventSeat { Id = 2, EventAreaId = 1, Number = 2, Row = 1, State = 0},
				new EventSeat { Id = 3, EventAreaId = 1, Number = 3, Row = 1, State = 0},
				new EventSeat { Id = 4, EventAreaId = 1, Number = 1, Row = 2, State = 0},
				new EventSeat { Id = 5, EventAreaId = 1, Number = 2, Row = 2, State = 1},
				new EventSeat { Id = 13, EventAreaId = 2, Number = 1, Row = 1, State = 1},
				new EventSeat { Id = 14, EventAreaId = 3, Number = 1, Row = 1, State = 0},
				new EventSeat { Id = 10, EventAreaId = 9, Number = 1, Row = 2, State = 0},
				new EventSeat { Id = 11, EventAreaId = 9, Number = 2, Row = 1, State = 0},
				new EventSeat { Id = 12, EventAreaId = 9, Number = 2, Row = 2, State = 0},
				new EventSeat { Id = 9, EventAreaId = 9, Number = 1, Row = 1, State = 0},
				new EventSeat { Id = 15, EventAreaId = 2, Number = 10, Row = 1, State =1},
				new EventSeat { Id = 16, EventAreaId = 1, Number = 14, Row = 3, State = 1}
			};
		}

		public void Create(EventSeat entity)
		{
			_list.Add(entity);
		}

		public void Delete(int id)
		{
			_list.Remove(_list.Find(x => x.Id == id));
		}

		public void DeleteBy(Expression<Func<EventSeat, bool>> expression)
		{
			_list.RemoveAll(new Predicate<EventSeat>(expression.Compile()));
		}

		public IQueryable<EventSeat> FindBy(Expression<Func<EventSeat, bool>> expression)
		{
			return _list.FindAll(new Predicate<EventSeat>(expression.Compile())).AsQueryable();
		}

		public EventSeat Get(int id)
		{
			return _list.FirstOrDefault(x => x.Id == id);
		}

		public IQueryable<EventSeat> GetList()
		{
			return _list.ToList().AsQueryable();
		}

		public void Update(EventSeat entity)
		{
			var update = _list.FirstOrDefault(x => x.Id == entity.Id);
			update.Row = entity.Row;
			update.Number = entity.Number;
			update.State = entity.State;
			update.EventAreaId = entity.EventAreaId;
		}
	}
}
