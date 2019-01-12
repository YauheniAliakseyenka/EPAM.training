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
	internal class AreaService : IStoreService<AreaDto, int>
	{
		private readonly IWorkUnit _context;
		private readonly IStoreService<SeatDto, int> _seatService;

		public AreaService(IWorkUnit context, IStoreService<SeatDto, int> seatService)
		{
			_context = context;
			_seatService = seatService;
		}

		public async Task Create(AreaDto entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			if (entity.LayoutId == 0)
				throw new AreaException("LayoutId equals zero");

			if(!IsDescriptionUnique(entity, false))
				throw new AreaException("Area description isn't unique");

			if(entity.SeatList == null || !entity.SeatList.Any())
				throw new AreaException("Incorrect state of area. An area must have atleast one seat");

			var areaAdd = mapToArea(entity);
			using (var transaction = CustomTransactionScope.GetTransactionScope())
			{
				_context.AreaRepository.Create(areaAdd);
				await _context.SaveAsync();
				entity.Id = areaAdd.Id;
				foreach (var seat in entity.SeatList)
				{
					seat.AreaId = areaAdd.Id;
					await _seatService.Create(seat);
				}

				transaction.Complete();
			}
		}

		public async Task Delete(int id)
		{
			_context.AreaRepository.Delete(id);
			await _context.SaveAsync();
		}

		public Task<IEnumerable<AreaDto>> FindBy(Expression<Func<AreaDto, bool>> expression)
		{
			var result = new List<AreaDto>();
			Expression<Func<Area, bool>> predicate = x => expression.Compile().Invoke(mapToAreaDto(x));
			var list = _context.AreaRepository.FindBy(predicate);

			return Task.FromResult(list.Select(x => mapToAreaDto(x)).AsEnumerable());
		}

		public async Task<AreaDto> Get(int id)
		{
			var area = await _context.AreaRepository.GetAsync(id);

			if (area == null)
				return null;

			return mapToAreaDto(area);
		}

		public async Task<IEnumerable<AreaDto>> GetList()
		{
			var temp = await _context.AreaRepository.GetListAsync();

			return temp.Select(x => mapToAreaDto(x));
		}

		public async Task Update(AreaDto entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			if (entity.LayoutId == 0)
				throw new AreaException("LayoutId equals zero");

			if (!IsDescriptionUnique(entity, false))
				throw new AreaException("Area description isn't unique");

			var update = await _context.AreaRepository.GetAsync(entity.Id);
			update.Description = entity.Description;
			update.CoordY = entity.CoordY;
			update.CoordX = entity.CoordX;
			_context.AreaRepository.Update(update);
			await _context.SaveAsync();
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

		private bool IsDescriptionUnique(AreaDto entity, bool isCreating)
		{
			return isCreating? 
               !_context.AreaRepository.FindBy(x => x.LayoutId == entity.LayoutId
                && x.Description.Equals(entity.Description, StringComparison.OrdinalIgnoreCase)).Any() :
               !_context.AreaRepository.FindBy(x => x.Id != entity.Id && (x.LayoutId == entity.LayoutId
                && x.Description.Equals(entity.Description, StringComparison.OrdinalIgnoreCase))).Any();
		}
	}
}
