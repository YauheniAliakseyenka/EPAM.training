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
	internal class SeatService: IService<SeatView>
	{
		private IRepository<Seat> _repo;

		public SeatService(IRepository<Seat> repo)
		{
			_repo = repo;
		}

		public int Create(SeatView entity)
		{
			if (entity == null)
				throw new NullReferenceException();

			if (entity.AreaId == 0)
				throw new SeatException("Area wasn't chosen");

			if (!SeatValdiator.isSeatUnique(entity, Find(x => x.AreaId == entity.AreaId)))
				throw new SeatException("Seat already exists");

			var seatAdd = new Seat()
			{
				AreaId = entity.AreaId,
				Number = entity.Number,
				Row = entity.Row,
				Id = entity.Id
			};

			_repo.Create(seatAdd);

			return 1;
		}

		public int Delete(int id)
		{
			return _repo.Delete(id);
		}

		public IEnumerable<SeatView> Find(Expression<Func<SeatView, bool>> expression)
		{
			return GetList().Where(expression.Compile()).ToList();
		}

		public SeatView Get(int id)
		{
			var seat = _repo.Get(id);

			if (seat == null)
				return null;

			var result = new SeatView()
			{
				AreaId = seat.AreaId,
				Number = seat.Number,
				Row = seat.Row,
				Id = seat.Id
			};

			return result;
		}

		public IEnumerable<SeatView> GetList()
		{
			var temp = _repo.GetList();

			if (temp == null)
				return null;

			var result = new List<SeatView>();
			temp.ToList().ForEach(x =>
			{
				result.Add(new SeatView()
				{
					AreaId = x.AreaId,
					Number = x.Number,
					Row = x.Row,
					Id = x.Id
				});
			});

			return result;
		}

		public void Update(SeatView entity)
		{
			if (entity == null)
				throw new NullReferenceException();

			if (entity.AreaId == 0)
				throw new SeatException("Area wasn't chosen");

			if (!SeatValdiator.isSeatUnique(entity, Find(x => x.AreaId == entity.AreaId)))
				throw new SeatException("Area description isnt' unique");

			var update = new Seat()
			{
				AreaId = entity.AreaId,
				Number = entity.Number,
				Row = entity.Row,
				Id = entity.Id
			};

			_repo.Update(update);
		}
	}
}
