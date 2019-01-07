using BusinessLogic.DTO;
using BusinessLogic.Exceptions;
using BusinessLogic.Exceptions.VenueExceptions;
using DataAccess;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BusinessLogic.Services
{
	internal class SeatService: IStoreService<SeatDto, int>
	{
		private IWorkUnit _context;

		public SeatService(IWorkUnit context)
		{
			_context = context;
		}

		public void Create(SeatDto entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			if (entity.AreaId == 0)
				throw new SeatException("AreaId equals zero");

			if (IsSeatNotUnique(entity))
				throw new SeatException("Seat already exists");

			var seatAdd = mapToSeat(entity);
			_context.SeatRepository.Create(seatAdd);
			_context.Save();
			entity.Id = seatAdd.Id;
		}

		public void Delete(int id)
		{
			_context.SeatRepository.Delete(id);
		}

		public IEnumerable<SeatDto> FindBy(Expression<Func<SeatDto, bool>> expression)
		{
			var result = new List<SeatDto>();
			Expression<Func<Seat, bool>> predicate = x => expression.Compile().Invoke(mapToSeatDto(x));
			var list = _context.SeatRepository.FindBy(predicate).ToList();

			if (!list.Any())
				return result;

			list.ForEach(x =>
			{
				result.Add(mapToSeatDto(x));
			});

			return result;
		}

		public SeatDto Get(int id)
		{
			var seat = _context.SeatRepository.Get(id);

			if (seat == null)
				return null;

			return mapToSeatDto(seat);
		}

		public IEnumerable<SeatDto> GetList()
		{
			var temp = _context.SeatRepository.GetList();

			if (temp == null)
				return null;

			var result = new List<SeatDto>();
			temp.ToList().ForEach(x =>
			{
				result.Add(mapToSeatDto(x));
			});

			return result;
		}

		public void Update(SeatDto entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			if (entity.AreaId == 0)
				throw new SeatException("AreaId equals zero");

			if (IsSeatNotUnique(entity))
				throw new SeatException("Area description isn't unique");

			var update = _context.SeatRepository.Get(entity.Id);
			update.Number = entity.Number;
			update.Row = entity.Row;
			_context.SeatRepository.Update(update);
			_context.Save();
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

		private bool IsSeatNotUnique(SeatDto entity)
		{
			return _context.SeatRepository.FindBy(c => c.AreaId == entity.AreaId &&
			(c.Row == entity.Row & c.Number == entity.Number)).Any();
		}
	}
}
