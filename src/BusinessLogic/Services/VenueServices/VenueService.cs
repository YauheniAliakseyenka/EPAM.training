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
	internal class VenueService : IStoreService<VenueDto, int>
	{
		private readonly IWorkUnit _context;
		private readonly IStoreService<LayoutDto, int> _layoutService;

		public VenueService(IWorkUnit context, IStoreService<LayoutDto, int> layoutService)
		{
			_context = context;
			_layoutService = layoutService;
		}

		public async Task Create(VenueDto entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			if (!IsNameUnique(entity, true))
				throw new VenueException("Such venue already exists");

			if(entity.LayoutList == null || !entity.LayoutList.Any())
				throw new VenueException("Incorrect state of the venue. The venue must have at least one layout");

			var venueAdd = mapToVenue(entity);	
			using (var transaction = CustomTransactionScope.GetTransactionScope())
			{
				_context.VenueRepository.Create(venueAdd);
				await _context.SaveAsync();
				entity.Id = venueAdd.Id;
				foreach (var layout in entity.LayoutList)
				{
					layout.VenueId = venueAdd.Id;
					await _layoutService.Create(layout);
				}
				transaction.Complete();
			}
		}

		public async Task Delete(int id)
		{
			_context.VenueRepository.Delete(id);
			await _context.SaveAsync();
		}

		public Task<IEnumerable<VenueDto>> FindBy(Expression<Func<VenueDto, bool>> expression)
		{
			var result = new List<VenueDto>();
			Expression<Func<Venue, bool>> predicate = x => expression.Compile().Invoke(mapToVenueDto(x));
			var list = _context.VenueRepository.FindBy(predicate);

			return Task.FromResult(list.Select(x => mapToVenueDto(x)).AsEnumerable());
		}

		public async Task<VenueDto> Get(int id)
		{
			var venue = await _context.VenueRepository.GetAsync(id);

			if(venue == null)
				return null;

			return mapToVenueDto(venue);
		}

		public async Task<IEnumerable<VenueDto>> GetList()
		{
			var list = await _context.VenueRepository.GetListAsync();

			return list.Select(x => mapToVenueDto(x));
		}

        public async Task Update(VenueDto entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			if (!IsNameUnique(entity, false))
				throw new VenueException("Such venue already exists");

			var update = await _context.VenueRepository.GetAsync(entity.Id);
			update.Name = entity.Name;
			update.Phone = entity.Phone;
			update.Description = entity.Description;
			update.Address = entity.Address;
			_context.VenueRepository.Update(update);
			await _context.SaveAsync();
		}

		private VenueDto mapToVenueDto(Venue from)
		{
			return new VenueDto
			{
				Address = from.Address,
				Description = from.Description,
				Id = from.Id,
				Name = from.Name,
				Phone = from.Phone,
				LayoutList = new List<LayoutDto>()
			};
		}

		private Venue mapToVenue(VenueDto from)
		{
			return new Venue
			{
				Address = from.Address,
				Description = from.Description,
				Id = from.Id,
				Name = from.Name,
				Phone = from.Phone
			};
		}

		private bool IsNameUnique(VenueDto entity, bool isCreating)
		{
            return isCreating ?
                !_context.VenueRepository.FindBy(y => y.Name.Equals(entity.Name, StringComparison.OrdinalIgnoreCase)).Any() :
                !_context.VenueRepository.FindBy(y => y.Id != entity.Id && y.Name.Equals(entity.Name, StringComparison.OrdinalIgnoreCase)).Any();
		}
	}
}
