using DataAccess;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Validators;
using System.Linq.Expressions;
using BusinessLogic.Exceptions;
using BusinessLogic.ViewEntities;

namespace BusinessLogic.Services
{
	internal class EventService : IService<EventView>
	{
		private IRepository<Event> _eventRepo;
		private IRepository<EventArea> _eventAreaRepo;

		public EventService(IRepository<Event> eventRepo, IRepository<EventArea> eventAreaRepo)
		{
			_eventRepo = eventRepo;
			_eventAreaRepo = eventAreaRepo;
		}

		public int Create(EventView entity)
		{
			if (entity == null)
				throw new NullReferenceException();

			if(entity.LayoutId == 0)
				throw new EventException("Layout wasn't chosen");

			if (!EventValidator.isDateValid(entity, GetList()))
				throw new EventException("Date isn't valid");

			if (!EventValidator.isDateNotPast(entity.Date))
				throw new EventException("Attempt of creating event with a date in the past");

			var add = new Event()
			{
				Date = entity.Date,
				Description = entity.Description,
				LayoutId = entity.LayoutId,
				Name = entity.Name,
				Id = entity.Id
			};

			var insertedId = _eventRepo.Create(add);
			entity.Id = insertedId;

			return 1;
		}

		public int Delete(int id)
		{
			return _eventRepo.Delete(id);
		}

		public IEnumerable<EventView> Find(Expression<Func<EventView, bool>> expression)
		{
			return GetList().Where(expression.Compile()).ToList();
		}

		public EventView Get(int id)
		{
			var temp = _eventRepo.Get(id);

			if (temp == null)
				return null;

			var result = new EventView()
			{
				Date = temp.Date,
				Description = temp.Description,
				LayoutId = temp.LayoutId,
				Name = temp.Name,
				EventAreaList = new List<int>(),
				Id = temp.Id
			};

			result.EventAreaList.AddRange(_eventAreaRepo.GetList().Where(x => x.EventId == id).Select(x => x.Id).ToList());

			return result;
		}

		public IEnumerable<EventView> GetList()
		{
			var temp = _eventRepo.GetList();

			if (temp == null)
				return null;

			var result = new List<EventView>();
			temp.ToList().ForEach(x =>
			{
				result.Add(new EventView()
				{
					Date = x.Date,
					Description = x.Description,
					LayoutId = x.LayoutId,
					Name = x.Name,
					EventAreaList = null,
					Id = x.Id
				});
			});

			return result;
		}

		public void Update(EventView entity)
		{
			if (entity == null)
				throw new NullReferenceException();

			if (entity.LayoutId == 0)
				throw new EventException("Layout wasn't chosen");

			if (!EventValidator.isDateNotPast(entity.Date))
				throw new EventException("Past date");

			var update = new Event()
			{
				Date = entity.Date,
				Description = entity.Description,
				LayoutId = entity.LayoutId,
				Name = entity.Name,
				Id = entity.Id
			};

			_eventRepo.Update(update);
		}
	}
}
