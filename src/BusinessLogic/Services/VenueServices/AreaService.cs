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
	internal class AreaService : IStoreService<AreaDto, int>
	{
		private readonly IWorkUnit _context;
		private readonly IStoreService<SeatDto, int> _seatService;

		public AreaService(IWorkUnit context, IStoreService<SeatDto, int> seatService)
		{
			_context = context;
			_seatService = seatService;
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

			if(entity.SeatList == null || !entity.SeatList.Any())
				throw new AreaException("Incorrect state of area. An area must have atleast one seat");

			var areaAdd = MapToArea(entity);
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
			var delete = await _context.AreaRepository.GetAsync(id);

			if (delete == null)
				return;

			_context.AreaRepository.Delete(delete);
			await _context.SaveAsync();
		}

		public async Task<AreaDto> Get(int id)
		{
			var area = await _context.AreaRepository.GetAsync(id);

			return area == null? null: MapToAreaDto(area);
		}

		public async Task<IEnumerable<AreaDto>> GetList()
		{
			var temp = await _context.AreaRepository.GetListAsync();

			return temp.Select(x => MapToAreaDto(x));
		}

		public async Task Update(AreaDto entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			if (entity.LayoutId <= 0)
				throw new AreaException("LayoutId is invalid");

			if (!IsDescriptionUnique(entity, false))
				throw new AreaException("Area description isn't unique");

			var update = await _context.AreaRepository.GetAsync(entity.Id);
			update.Description = entity.Description;
			update.CoordY = entity.CoordY;
			update.CoordX = entity.CoordX;
			_context.AreaRepository.Update(update);
			await _context.SaveAsync();
		}

		private AreaDto MapToAreaDto(Area from)
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

		private Area MapToArea(AreaDto from)
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
	}
}
