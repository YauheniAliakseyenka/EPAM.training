using BusinessLogic.Exceptions;
using BusinessLogic.Validators;
using BusinessLogic.ViewEntities;
using DataAccess;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
	internal class EventSeatService : IService<EventSeatView>
	{
		private IRepository<EventSeat> _repo;

		public EventSeatService(IRepository<EventSeat> repo)
		{
			_repo = repo;
		}

		public int Create(EventSeatView entity)
		{
			if (entity == null)
				throw new NullReferenceException();

			if (entity.EventAreaId == 0)
				throw new EventSeatException("Area wasn't chosen");

			if (!EventSeatValidator.isSeatUnique(entity, Find(x => x.EventAreaId == entity.EventAreaId)))
				throw new EventSeatException("Seat already exists");

			var add = new EventSeat()
			{
				Row = entity.Row,
				Number = entity.Number,
				EventAreaId = entity.EventAreaId,
				State = entity.State,
				Id = entity.Id
			};

			_repo.Create(add);

			return 1;
		}

		public int Delete(int id)
		{
			return _repo.Delete(id);
		}

		public IEnumerable<EventSeatView> Find(Expression<Func<EventSeatView, bool>> expression)
		{
			return GetList().Where(expression.Compile()).ToList();
		}

		public EventSeatView Get(int id)
		{
			var seat = _repo.Get(id);

			if (seat == null)
				return null;

			var result = new EventSeatView()
			{
				Row = seat.Row,
				Number = seat.Number,
				EventAreaId = seat.EventAreaId,
				State = seat.State,
				Id = seat.Id
			};

			return result;
		}

		public IEnumerable<EventSeatView> GetList()
		{
			var temp = _repo.GetList();

			if (temp == null)
				return null;

			var result = new List<EventSeatView>();
			temp.ToList().ForEach(x =>
			{
				result.Add(new EventSeatView()
				{
					Row = x.Row,
					Number = x.Number,
					EventAreaId = x.EventAreaId,
					State = x.State,
					Id = x.Id
				});
			});

			return result;
		}

		public void Update(EventSeatView entity)
		{
			if (entity == null)
				throw new NullReferenceException();

			if (entity.EventAreaId == 0)
				throw new EventSeatException("Area wasn't chosen");

			if (!EventSeatValidator.isSeatUnique(entity, Find(x => x.EventAreaId == entity.EventAreaId)))
				throw new EventSeatException("Seat already exists");

			var update = new EventSeat()
			{
				Row = entity.Row,
				Number = entity.Number,
				EventAreaId = entity.EventAreaId,
				State = entity.State,
				Id = entity.Id
			};

			_repo.Update(update);
		}

	}
}
