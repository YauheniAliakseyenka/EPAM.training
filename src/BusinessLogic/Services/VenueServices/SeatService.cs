using BusinessLogic.DTO;
using BusinessLogic.Exceptions.VenueExceptions;
using DataAccess;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
	internal class SeatService: IStoreService<SeatDto, int>
	{
		private IWorkUnit _context;

		public SeatService(IWorkUnit context)
		{
			_context = context;
		}

		public async Task Create(SeatDto entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			if (entity.AreaId <= 0)
				throw new SeatException("AreaId is invalid");

			if (!IsSeatUnique(entity, true))
				throw new SeatException("Seat already exists");

			var seatAdd = MapToSeat(entity);
			_context.SeatRepository.Create(seatAdd);
			await _context.SaveAsync();
			entity.Id = seatAdd.Id;
		}

		public async Task Delete(int id)
		{
			var delete = await _context.SeatRepository.GetAsync(id);

			if (delete == null)
				return;

			_context.SeatRepository.Delete(delete);
			await _context.SaveAsync();
		}

		public async Task<SeatDto> Get(int id)
		{
			var seat = await _context.SeatRepository.GetAsync(id);

			return seat == null? null : MapToSeatDto(seat);
		}

		public async Task<IEnumerable<SeatDto>> GetList()
		{
			var list = await _context.SeatRepository.GetListAsync();

			return list.Select(x => MapToSeatDto(x));
		}

		public async Task Update(SeatDto entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			if (entity.AreaId <= 0)
				throw new SeatException("AreaId is invalid");

			if (!IsSeatUnique(entity, false))
				throw new SeatException("Area description isn't unique");

			var update = await _context.SeatRepository.GetAsync(entity.Id);
			update.Number = entity.Number;
			update.Row = entity.Row;
			_context.SeatRepository.Update(update);
			await _context.SaveAsync();
		}

		private SeatDto MapToSeatDto(Seat from)
		{
			return new SeatDto
			{
				AreaId = from.AreaId,
				Id = from.Id,
				Number = from.Number,
				Row = from.Number
			};
		}

		private Seat MapToSeat(SeatDto from)
		{
			return new Seat
			{
				AreaId = from.AreaId,
				Id = from.Id,
				Number = from.Number,
				Row = from.Number
			};
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
