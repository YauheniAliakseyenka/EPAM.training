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
	internal class EventAreaService : IService<EventAreaView>
	{
		private IRepository<EventArea> _eventAreaRepo;
		private IRepository<EventSeat> _eventSeatRepo;

		public EventAreaService(IRepository<EventArea> eventAreaRepo, IRepository<EventSeat> eventSeatRepo)
		{
			_eventAreaRepo = eventAreaRepo;
			_eventSeatRepo = eventSeatRepo;
		}

		public int Create(EventAreaView entity)
		{
			if (entity == null)
				throw new NullReferenceException();

			if (entity.EventId == 0)
				throw new EventAreaException("Event wasn't chosen");

			if (!EventAreaValidator.IsDescriptionUnique(entity.Description, Find(x => x.EventId == entity.EventId)))
				throw new EventAreaException("Area description isn't unique");

			var add = new EventArea()
			{
				CoordX = entity.CoordX,
				CoordY = entity.CoordY,
				Description = entity.Description,
				EventId = entity.EventId,
				Price = entity.Price,
				AreaDefaultId = entity.AreaDefaultId,
				Id = entity.Id
			};

			_eventAreaRepo.Create(add);

			return 1;
		}

		public int Delete(int id)
		{
			return _eventAreaRepo.Delete(id);
		}

		public IEnumerable<EventAreaView> Find(Expression<Func<EventAreaView, bool>> expression)
		{
			return GetList().Where(expression.Compile()).ToList();
		}

		public EventAreaView Get(int id)
		{
			var area = _eventAreaRepo.Get(id);

			if (area == null)
				return null;

			var result = new EventAreaView()
			{
				CoordX = area.CoordX,
				CoordY = area.CoordY,
				Description = area.Description,
				EventId = area.EventId,
				EventSeatList = new List<int>(),
				Price = area.Price,
				AreaDefaultId = area.AreaDefaultId,
				Id = area.Id
			};

			result.EventSeatList.AddRange(_eventSeatRepo.GetList().Where(x => x.EventAreaId == id).Select(x => x.Id).ToList());

			return result;
		}

		public IEnumerable<EventAreaView> GetList()
		{
			var temp = _eventAreaRepo.GetList();

			if (temp == null)
				return null;

			var result = new List<EventAreaView>();
			temp.ToList().ForEach(x =>
			{
				result.Add(new EventAreaView()
				{
					CoordX = x.CoordX,
					CoordY = x.CoordY,
					Description = x.Description,
					EventId = x.EventId,
					EventSeatList = new List<int>(),
					Price = x.Price,
					AreaDefaultId = x.AreaDefaultId,
					Id = x.Id
				});
			});

			return result;
		}

		public void Update(EventAreaView entity)
		{
			var update = new EventArea()
			{
				CoordX = entity.CoordX,
				CoordY = entity.CoordY,
				Description = entity.Description,
				EventId = entity.EventId,
				Price = entity.Price,
				AreaDefaultId = entity.AreaDefaultId,
				Id = entity.Id
			};

			_eventAreaRepo.Update(update);
		}
	}
}
