using BusinessLogic.DTO;
using BusinessLogic.Exceptions.VenueExceptions;
using DataAccess;
using BusinessLogic.Parsers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace BusinessLogic.Services
{
	internal class AreaService : IStoreService<AreaDto, int>
	{
		private readonly IWorkUnit _context;

		public bool IsValidatedToCreate { get; private set; }

		public AreaService(IWorkUnit context)
		{
			_context = context;
		}

        /// <summary>
        /// Create an area. An area can not be created without seats
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
		public async Task Create(AreaDto entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			if (entity.LayoutId <= 0)
				throw new AreaException("LayoutId is invalid");

			if (!IsDescriptionUnique(entity, true))
				throw new AreaException("Area description isn't unique");

			if (entity.SeatList == null || !entity.SeatList.Any())
				throw new AreaException("Incorrect state of area. An area must have atleast one seat");

			var areaAdd = AreaParser.MapToArea(entity);
			using (var transaction = CustomTransactionScope.GetTransactionScope())
			{
				_context.AreaRepository.Create(areaAdd);
				await _context.SaveAsync();
				entity.Id = areaAdd.Id;
				foreach (var seat in entity.SeatList)
				{
					seat.AreaId = areaAdd.Id;

					if (!IsSeatUnique(seat, false))
						throw new SeatException("Seat already exists");

					var seatAdd = SeatParser.MapToSeat(seat);
					_context.SeatRepository.Create(seatAdd);
				}
				await _context.SaveAsync();
				transaction.Complete();
			}
		}

		public async Task Delete(int id)
		{
			var delete = await _context.AreaRepository.GetAsync(id);

			if (delete == null)
				return;

			_context.AreaRepository.Delete(delete);
			await _context.SaveAsync();
		}

		public async Task<AreaDto> Get(int id)
		{
			var area = await _context.AreaRepository.GetAsync(id);

			return area == null? null: AreaParser.MapToAreaDto(area);
		}

		public async Task<IEnumerable<AreaDto>> GetList()
		{
			var temp = await _context.AreaRepository.GetListAsync();

			return temp.Select(x => AreaParser.MapToAreaDto(x));
		}

		public async Task Update(AreaDto entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			if (entity.LayoutId <= 0)
				throw new AreaException("LayoutId is invalid");

			if (!IsDescriptionUnique(entity, false))
				throw new AreaException("Area description isn't unique");

			if (entity.SeatList == null || !entity.SeatList.Any())
				throw new AreaException("Incorrect state of area. An area must have atleast one seat");

			using (var transaction = CustomTransactionScope.GetTransactionScope())
			{
				var delete = await _context.AreaRepository.GetAsync(entity.Id);
				_context.AreaRepository.Delete(delete);
				var createNew = AreaParser.MapToArea(entity);
				_context.AreaRepository.Create(createNew);
				await _context.SaveAsync();

				foreach (var seat in entity.SeatList)
				{
					seat.AreaId = createNew.Id;

					if (!IsSeatUnique(seat, false))
						throw new SeatException("Seat already exists");

					var seatAdd = SeatParser.MapToSeat(seat);
					_context.SeatRepository.Create(seatAdd);
				}
				await _context.SaveAsync();

				transaction.Complete();
			}
		}

		private bool IsDescriptionUnique(AreaDto entity, bool isCreating)
		{
			var data = from areas in _context.AreaRepository.GetList()
					   where areas.LayoutId == entity.LayoutId &&
					   areas.Description.Equals(entity.Description, StringComparison.OrdinalIgnoreCase)
					   select areas;

			return isCreating ? 
				!data.Any() :
				!(from areas in data
				 where areas.Id != entity.Id
				 select areas).Any();
		}

		private bool IsSeatUnique(SeatDto entity, bool isCreating)
		{
			var data = from seats in _context.SeatRepository.GetList()
					   where seats.AreaId == entity.AreaId && seats.Row == entity.Row && seats.Number == entity.Number
					   select seats;

			return isCreating ?
				!data.Any() :
				!(from seats in data
				  where seats.Id != entity.Id
				  select seats).Any();
		}
	}
}
