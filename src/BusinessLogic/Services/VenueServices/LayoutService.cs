using BusinessLogic.DTO;
using BusinessLogic.Exceptions.VenueExceptions;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Parsers;

namespace BusinessLogic.Services
{
	internal class LayoutService : ILayoutService
	{
		private readonly IWorkUnit _context;
		private readonly IStoreService<AreaDto, int> _areaService;

		public bool IsValidatedToCreate { get; private set; }

		public LayoutService(IWorkUnit context, IStoreService<AreaDto, int> areaService)
		{
			_context = context;
			_areaService = areaService;
		}

        /// <summary>
        /// Create a layout. A layout can not be created without areas
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
		public async Task Create(LayoutDto entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			if (entity.VenueId <= 0)
				throw new LayoutException("VenueId is invalid");

			if (!IsDescriptionUnique(entity, true))
				throw new LayoutException("Layout description isn't unique");

			if (entity.AreaList == null || !entity.AreaList.Any())
				throw new LayoutException("Incorrect state of the layout. The layout must have at least one area");

			var layoutAdd = LayoutParser.MapToLayout(entity);
			using (var transaction = CustomTransactionScope.GetTransactionScope())
			{
				_context.LayoutRepository.Create(layoutAdd);
				await _context.SaveAsync();
				entity.Id = layoutAdd.Id;
				foreach (var area in entity.AreaList)
				{
					area.LayoutId = layoutAdd.Id;
					await _areaService.Create(area);
				}

				transaction.Complete();
			}
		}

		public async Task Delete(int id)
		{
			var delete = await _context.LayoutRepository.GetAsync(id);

			if (delete == null)
				return;

			_context.LayoutRepository.Delete(delete);
			await _context.SaveAsync();
		}

		public async Task<LayoutDto> Get(int id)
		{
			var layout = await _context.LayoutRepository.GetAsync(id);
            
			return layout == null? null: LayoutParser.MapToLayoutDto(layout);
		}

		public  async Task<IEnumerable<LayoutDto>> GetList()
		{
			var list = await _context.LayoutRepository.GetListAsync();

			return list.Select(x => LayoutParser.MapToLayoutDto(x));
		}

		public async Task Update(LayoutDto entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			if (entity.VenueId <= 0)
				throw new LayoutException("VenueId is invalid");

			if (!IsDescriptionUnique(entity, false))
				throw new LayoutException("Area description isn't unique");

			if (entity.AreaList == null || !entity.AreaList.Any())
				throw new LayoutException("Incorrect state of the layout. The layout must have at least one area");

			using (var transaction = CustomTransactionScope.GetTransactionScope())
			{
				var update = await _context.LayoutRepository.GetAsync(entity.Id);
				update.VenueId = entity.VenueId;
				update.Description = entity.Description;
				_context.LayoutRepository.Update(update);
				await _context.SaveAsync();

				var existingAreas = await _context.AreaRepository.FindByAsync(x => x.LayoutId == entity.Id);

				//find and remove layouts which were deleted
				existingAreas.Where(list2 => entity.AreaList.All(list1 => list1.Id != list2.Id)).ToList()
					.ForEach(x =>
					{
						_context.AreaRepository.Delete(x);
					});

				foreach (var area in entity.AreaList)
				{
					if (area.Id == 0)
					{
						area.LayoutId = update.Id;
						await _areaService.Create(area);
					}
					else
						await _areaService.Update(area);
				}

				transaction.Complete();
			}
		}

		private bool IsDescriptionUnique(LayoutDto entity, bool isCreating)
		{
			var data = from layouts in _context.LayoutRepository.GetList()
					   where layouts.VenueId == entity.VenueId && layouts.Description.Equals(entity.Description, StringComparison.OrdinalIgnoreCase)
					   select layouts;

			return isCreating ? 
				!data.Any() :
                !(from layouts in data
				  where layouts.Id != entity.Id
				  select layouts).Any();
		}

		public Task<IEnumerable<LayoutDto>> GetLayoutsByVenue(int venueId)
		{
			var data = (from layouts in _context.LayoutRepository.GetList()
						where layouts.VenueId == venueId
						select layouts).ToList();

			return Task.FromResult(data.Select(x=> LayoutParser.MapToLayoutDto(x)));
		}
	}
}
