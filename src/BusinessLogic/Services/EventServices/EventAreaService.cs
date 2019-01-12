using BusinessLogic.DTO;
using BusinessLogic.Exceptions.EventExceptions;
using DataAccess;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

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

		public async Task Create(EventAreaDto entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			if (entity.EventId <= 0)
				throw new EventAreaException("EventId equals zero");

			if (!isDescriptionUnique(entity, true))
				throw new EventAreaException("Area description isn't unique");

			if (entity.Seats == null || !entity.Seats.Any())
				throw new EventAreaException("Invalid state of event area. Seat list is empty");

			using (var transaction = CustomTransactionScope.GetTransactionScope())
			{
				var add = mapToEventArea(entity);
				_context.EventAreaRepository.Create(add);
				await _context.SaveAsync();
				foreach (var seat in entity.Seats)
				{
					seat.EventAreaId = add.Id;
					await _seatService.Create(seat);
				}					

				entity.Id = add.Id;
				transaction.Complete();
			}
		}

		public async Task Delete(int id)
		{
			if (hasLockedSeats(id))
				throw new EventAreaException("Not allowed to delete. Area has locked seat");

			_context.EventAreaRepository.Delete(id);
			await _context.SaveAsync();
		}

		public Task<IEnumerable<EventAreaDto>> FindBy(Expression<Func<EventAreaDto, bool>> expression)
		{
			Expression<Func<EventArea, bool>> predicate = x => expression.Compile().Invoke(mapToEventAreaDto(x));
			var list =  _context.EventAreaRepository.FindBy(predicate);

			var result = list.Select(x => mapToEventAreaDto(x)).ToList();
			result.Sort();

			return Task.FromResult(result.AsEnumerable());
		}

		public async Task<EventAreaDto> Get(int id)
		{
            var area = await _context.EventAreaRepository.GetAsync(id);

			if (area == null)
				return null;

			return mapToEventAreaDto(area);
		}

		public async Task<IEnumerable<EventAreaDto>> GetList()
		{
			var tempAreaList = await _context.EventAreaRepository.GetListAsync();

			return tempAreaList.Select(x=>mapToEventAreaDto(x));
		}

		public async Task Update(EventAreaDto entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			if (entity.EventId == 0)
				throw new EventAreaException("EventId equals zero");

            if (!isDescriptionUnique(entity, false))
                throw new EventAreaException("Area description isn't unique");

            if (!entity.Seats.Any())
                throw new EventAreaException("Invalid state of event area. Seat list is empty");           

            using (var transaction = CustomTransactionScope.GetTransactionScope())
            {
				var update = await _context.EventAreaRepository.GetAsync(entity.Id);
				update.Price = entity.Price;
				update.Description = entity.Description;
				update.CoordY = entity.CoordY;
				update.CoordX = entity.CoordX;
				_context.EventAreaRepository.Update(update);
                await _context.SaveAsync();

				//find seats which were deleted
				var seats = await _seatService.FindBy(x => x.EventAreaId == update.Id);
				var differences = seats.Where(list2 => entity.Seats.All(list1 => list1.Id != list2.Id)).ToList();

                foreach(var seat in differences)
                    await _seatService.Delete(seat.Id);

				foreach (var seat in entity.Seats)
                {
                    seat.EventAreaId = update.Id;
                    if(seat.Id == 0)
						await _seatService.Create(seat);
					else
						await _seatService.Update(seat);
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
