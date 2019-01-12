using BusinessLogic.DTO;
using BusinessLogic.Exceptions.VenueExceptions;
using DataAccess;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

			if (entity.AreaId == 0)
				throw new SeatException("AreaId equals zero");

			if (!IsSeatUnique(entity, true))
				throw new SeatException("Seat already exists");

			var seatAdd = mapToSeat(entity);
			_context.SeatRepository.Create(seatAdd);
			await _context.SaveAsync();
			entity.Id = seatAdd.Id;
		}

		public async Task Delete(int id)
		{
			_context.SeatRepository.Delete(id);
			await _context.SaveAsync();
		}

		public Task<IEnumerable<SeatDto>> FindBy(Expression<Func<SeatDto, bool>> expression)
		{
			var result = new List<SeatDto>();
			Expression<Func<Seat, bool>> predicate = x => expression.Compile().Invoke(mapToSeatDto(x));
			var list = _context.SeatRepository.FindBy(predicate);

			return Task.FromResult(list.Select(x => mapToSeatDto(x)).AsEnumerable());
		}

		public async Task<SeatDto> Get(int id)
		{
			var seat = await _context.SeatRepository.GetAsync(id);

			if (seat == null)
				return null;

			return mapToSeatDto(seat);
		}

		public async Task<IEnumerable<SeatDto>> GetList()
		{
			var list = await _context.SeatRepository.GetListAsync();

			return list.Select(x => mapToSeatDto(x));
		}

		public async Task Update(SeatDto entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			if (entity.AreaId == 0)
				throw new SeatException("AreaId equals zero");

			if (!IsSeatUnique(entity, false))
				throw new SeatException("Area description isn't unique");

			var update = await _context.SeatRepository.GetAsync(entity.Id);
			update.Number = entity.Number;
			update.Row = entity.Row;
			_context.SeatRepository.Update(update);
			await _context.SaveAsync();
		}

		private SeatDto mapToSeatDto(Seat from)
		{
			return new SeatDto
			{
				AreaId = from.AreaId,
				Id = from.Id,
				Number = from.Number,
				Row = from.Number
			};
		}

		private Seat mapToSeat(SeatDto from)
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
            return isCreating ?
                !_context.SeatRepository.FindBy(c => c.AreaId == entity.AreaId &&
            (c.Row == entity.Row && c.Number == entity.Number)).Any() :
            !_context.SeatRepository.FindBy(c => c.Id != entity.Id &&
            (c.AreaId == entity.AreaId && (c.Row == entity.Row && c.Number == entity.Number))).Any();
        }
	}
}
