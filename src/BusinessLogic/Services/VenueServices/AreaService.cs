using BusinessLogic.DTO;
using BusinessLogic.Exceptions.VenueExceptions;
using DataAccess;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BusinessLogic.Services
{
	internal class AreaService : IStoreService<AreaDto, int>
	{
		private readonly IWorkUnit _context;
		private readonly IStoreService<SeatDto, int> _seatService;

		public AreaService(IWorkUnit context, IStoreService<SeatDto, int> seatService)
		{
			_context = context;
			_seatService = seatService;
		}

		public void Create(AreaDto entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			if (entity.LayoutId == 0)
				throw new AreaException("LayoutId equals zero");

			if(IsDescriptionNotUnique(entity.Description))
				throw new AreaException("Area description isn't unique");

			if(entity.SeatList == null || !entity.SeatList.Any())
				throw new AreaException("Incorrect state of the area. The area must have at least one seat");

			var areaAdd = mapToArea(entity);
			using (var transaction = CustomTransactionScope.GetTransactionScope())
			{
				_context.AreaRepository.Create(areaAdd);
				_context.Save();
				entity.Id = areaAdd.Id;
				foreach (var seat in entity.SeatList)
				{
					seat.AreaId = areaAdd.Id;
					_seatService.Create(seat);
				}

				transaction.Complete();
			}
		}

		public void Delete(int id)
		{
			_context.AreaRepository.Delete(id);
		}

		public IEnumerable<AreaDto> FindBy(Expression<Func<AreaDto, bool>> expression)
		{
			var result = new List<AreaDto>();
			Expression<Func<Area, bool>> predicate = x => expression.Compile().Invoke(mapToAreaDto(x));
			var list = _context.AreaRepository.FindBy(predicate).ToList();

			if (!list.Any())
				return result;

			list.ForEach(x =>
			{
				result.Add(mapToAreaDto(x));
			});

			return result;
		}

		public AreaDto Get(int id)
		{
			var area = _context.AreaRepository.Get(id);

			if (area == null)
				return null;

			return mapToAreaDto(area);
		}

		public IEnumerable<AreaDto> GetList()
		{
			var temp = _context.AreaRepository.GetList();

			if (temp == null)
				return null;

			var result = new List<AreaDto>();
			temp.ToList().ForEach(x =>
			{
				result.Add(mapToAreaDto(x));
			});

			return result;
		}

		public void Update(AreaDto entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			if (entity.LayoutId == 0)
				throw new AreaException("LayoutId equals zero");

			if (IsDescriptionNotUnique(entity.Description))
				throw new AreaException("Area description isn't unique");

			var update = _context.AreaRepository.Get(entity.Id);
			update.Description = entity.Description;
			update.CoordY = entity.CoordY;
			update.CoordX = entity.CoordX;
			_context.AreaRepository.Update(update);
			_context.Save();
		}

		private AreaDto mapToAreaDto(Area from)
		{
			return new AreaDto
			{
				SeatList = new List<SeatDto>(),
				CoordX = from.CoordX,
				CoordY = from.CoordY,
				Description = from.Description,
				Id = from.Id,
				LayoutId = from.LayoutId
			};
		}

		private Area mapToArea(AreaDto from)
		{
			return new Area
			{
				CoordX = from.CoordX,
				CoordY = from.CoordY,
				Description = from.Description,
				Id = from.Id,
				LayoutId = from.LayoutId
			};
		}

		private bool IsDescriptionNotUnique(string description)
		{
			return _context.AreaRepository.FindBy(x => x.Description.Equals(description, StringComparison.OrdinalIgnoreCase)).Any();
		}
	}
}
