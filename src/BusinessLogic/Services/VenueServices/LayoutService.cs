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
	internal class LayoutService : IStoreService<LayoutDto, int> 
	{
		private readonly IWorkUnit _context;
		private readonly IStoreService<AreaDto, int> _areaService;

		public LayoutService(IWorkUnit context, IStoreService<AreaDto, int> areaService)
		{
			_context = context;
			_areaService = areaService;
		}

		public async Task Create(LayoutDto entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			if (entity.VenueId == 0)
				throw new LayoutException("VenueID equals zero");

			if (!IsDescriptionUnique(entity, true))
				throw new LayoutException("Layout description isn't unique");

			if (entity.AreaList == null || !entity.AreaList.Any())
				throw new LayoutException("Incorrect state of the layout. The layout must have at least one area");

			var layoutAdd = mapToLayout(entity);
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
			_context.LayoutRepository.Delete(id);
			await _context.SaveAsync();
		}

		public Task<IEnumerable<LayoutDto>> FindBy(Expression<Func<LayoutDto, bool>> expression)
		{
			var result = new List<LayoutDto>();
			Expression<Func<Layout, bool>> predicate =  x =>  expression.Compile().Invoke(mapToLayoutDto(x));
			var list = _context.LayoutRepository.FindBy(predicate);

			return Task.FromResult(list.Select(x => mapToLayoutDto(x)).AsEnumerable());
		}

		public async Task<LayoutDto> Get(int id)
		{
			var layout = await _context.LayoutRepository.GetAsync(id);

			if (layout == null)
				return null;

			return mapToLayoutDto(layout);
		}

		public  async Task<IEnumerable<LayoutDto>> GetList()
		{
			var list = await _context.LayoutRepository.GetListAsync();

			return list.Select(x => mapToLayoutDto(x));
		}

		public async Task Update(LayoutDto entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			if (entity.VenueId == 0)
				throw new LayoutException("VenueId equals zero");

			if (!IsDescriptionUnique(entity, false))
				throw new LayoutException("Area description isn't unique");

			var update = await _context.LayoutRepository.GetAsync(entity.Id);
			update.VenueId = entity.VenueId;
			update.Description = entity.Description;
			_context.LayoutRepository.Update(update);
			await _context.SaveAsync();
		}

		private LayoutDto mapToLayoutDto(Layout from)
		{
			return new LayoutDto
			{
				AreaList = new List<AreaDto>(),
				Description = from.Description,
				Id = from.Id,
				VenueId = from.VenueId
			};
		}

		private Layout mapToLayout(LayoutDto from)
		{
			return new Layout
			{
				Description = from.Description,
				Id = from.Id,
				VenueId = from.VenueId
			};
		}

		private bool IsDescriptionUnique(LayoutDto entity, bool isCreating)
		{
            return isCreating ?
                !_context.LayoutRepository.FindBy(x => x.VenueId == entity.VenueId &&
                x.Description.Equals(entity.Description, StringComparison.OrdinalIgnoreCase)).Any() :
                !_context.LayoutRepository.FindBy(x => x.Id != entity.Id && (x.VenueId == entity.VenueId &&
                x.Description.Equals(entity.Description, StringComparison.OrdinalIgnoreCase))).Any();
		}
	}
}
