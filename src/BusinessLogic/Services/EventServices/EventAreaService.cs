using BusinessLogic.DTO;
using BusinessLogic.Exceptions.EventExceptions;
using DataAccess;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BusinessLogic.Services.EventServices
{
	internal class EventAreaService : IStoreService<EventAreaDto, int>
	{
		private readonly IWorkUnit _context;
        private IStoreService<EventSeatDto, int> _seatService;

		public EventAreaService(IWorkUnit context, IStoreService<EventSeatDto, int> seatService)
		{
			_context = context;
            _seatService = seatService;
		}

		public void Create(EventAreaDto entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			if (entity.EventId == 0)
				throw new EventAreaException("EventId equals zero");

			if (!isDescriptionUnique(entity, true))
				throw new EventAreaException("Area description isn't unique");

			if (entity.Seats == null || !entity.Seats.Any())
				throw new EventAreaException("Invalid state of event area. Seat list is empty");

			using (var transaction = CustomTransactionScope.GetTransactionScope())
			{
				var add = mapToEventArea(entity);
				_context.EventAreaRepository.Create(add);
				_context.Save();
				foreach (var seat in entity.Seats)
				{
					seat.EventAreaId = add.Id;
					_seatService.Create(seat);
				}					

				entity.Id = add.Id;
				transaction.Complete();
			}
		}

		public void Delete(int id)
		{
			if (hasLockedSeats(id))
				throw new EventAreaException("Not allowed to delete. Area has locked seat");

			_context.EventAreaRepository.Delete(id);
			_context.Save();
		}

		public IEnumerable<EventAreaDto> FindBy(Expression<Func<EventAreaDto, bool>> expression)
		{
			var result = new List<EventAreaDto>();

			Expression<Func<EventArea, bool>> predicate = x => expression.Compile().Invoke(mapToEventAreaDto(x));
			var list = _context.EventAreaRepository.FindBy(predicate);

			if (!list.Any())
				return result;

			foreach(var item in list)
			{
				var entity = mapToEventAreaDto(item);
				result.Add(entity);
			}
			result.Sort();

			return result;
		}

		public EventAreaDto Get(int id)
		{
			var area = _context.EventAreaRepository.Get(id);

			if (area == null)
				return null;

			return mapToEventAreaDto(area);
		}

		public IEnumerable<EventAreaDto> GetList()
		{
			var tempAreaList = _context.EventAreaRepository.GetList();

			if (tempAreaList == null)
				return null;

			var result = new List<EventAreaDto>();
			tempAreaList.ToList().ForEach(x =>
			{
				result.Add(mapToEventAreaDto(x));
			});

			return result;
		}

		public void Update(EventAreaDto entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			if (entity.EventId == 0)
				throw new EventAreaException("EventId equals zero");

            if(!entity.Seats.Any())
                throw new EventAreaException("Invalid state of event area. Seat list is empty");

            if (!isDescriptionUnique(entity, false))
				throw new EventAreaException("Area description isn't unique");

            using (var transaction = CustomTransactionScope.GetTransactionScope())
            {
				var update = _context.EventAreaRepository.Get(entity.Id);
				update.Price = entity.Price;
				update.Description = entity.Description;
				update.CoordY = entity.CoordY;
				update.CoordX = entity.CoordX;
				_context.EventAreaRepository.Update(update);
                _context.Save();

				//find seats which were deleted
				var differences = _seatService.FindBy(x => x.EventAreaId == update.Id).
					Where(list2 => entity.Seats.All(list1 => list1.Id != list2.Id));
				differences.ToList().ForEach(x =>
				{
					_seatService.Delete(x.Id);
				});

				foreach (var seat in entity.Seats)
                {
                    seat.EventAreaId = update.Id;
                    if(seat.Id == 0)
						_seatService.Create(seat);
					else
						_seatService.Update(seat);
				}
				
                transaction.Complete();
            }
		}
		
		private EventAreaDto mapToEventAreaDto(EventArea from)
		{
			return new EventAreaDto
			{
				EventId = from.EventId,
				Price = from.Price,
				Seats = new List<EventSeatDto>(),
				AreaDefaultId = from.AreaDefaultId,
				CoordX = from.CoordX,
				CoordY = from.CoordY,
				Description = from.Description,
				Id = from.Id
			};
		}

		private EventArea mapToEventArea(EventAreaDto from)
		{
			return new EventArea
			{
				EventId = from.EventId,
				Price = from.Price,
				AreaDefaultId = from.AreaDefaultId,
				CoordX = from.CoordX,
				CoordY = from.CoordY,
				Description = from.Description,
				Id = from.Id
			};
		}

		private bool isDescriptionUnique(EventAreaDto entity, bool isCreating)
		{
			bool isUnique = isCreating ? !(from areas in _context.EventAreaRepository.GetList()
										   where areas.EventId == entity.EventId &&
										   areas.Description.ToLower() == entity.Description.ToLower()
										   select areas).Any() :
								   !(from areas in _context.EventAreaRepository.GetList()
									 where areas.Id != entity.Id && (areas.EventId == entity.EventId &&
									 areas.Description.ToLower() == entity.Description.ToLower())
									 select areas).Any();

			return isUnique;
		}

		private bool hasLockedSeats(int id)
		{
			var hasLocked = (from areas in _context.EventAreaRepository.GetList()
							 join seats in _context.EventSeatRepository.GetList() on areas.Id equals seats.EventAreaId
							 where  seats.EventAreaId == id && seats.State == 1
							 select seats).Any();

			return hasLocked;
		}
	}
}
