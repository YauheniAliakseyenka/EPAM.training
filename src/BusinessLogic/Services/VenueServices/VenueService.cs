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
	internal class VenueService : IStoreService<VenueDto, int>
	{
		private readonly IWorkUnit _context;
		private readonly ILayoutService _layoutService;

		public bool IsValidatedToCreate { get; private set; }

		public VenueService(IWorkUnit context, ILayoutService layoutService)
		{
			_context = context;
			_layoutService = layoutService;
		}

        /// <summary>
        /// Create a venue. A venue can not be created without layouts
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task Create(VenueDto entity)
        {
            if (entity == null)
                throw new ArgumentNullException();

            if (!IsNameUnique(entity, true))
                throw new VenueException("Such venue already exists");

            if (entity.LayoutList == null || !entity.LayoutList.Any())
                throw new VenueException("Incorrect state of the venue. The venue must have at least one layout");

            var venueAdd = MapToVenue(entity);
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
			var delete = await _context.VenueRepository.GetAsync(id);

			if (delete == null)
				return;

			_context.VenueRepository.Delete(delete);
			await _context.SaveAsync();
		}

		public async Task<VenueDto> Get(int id)
		{
			var venue = await _context.VenueRepository.GetAsync(id);

            return venue == null ? null : MapToVenueDto(venue);
		}

		public async Task<IEnumerable<VenueDto>> GetList()
		{
			var list = await _context.VenueRepository.GetListAsync();

			return list.Select(x => MapToVenueDto(x));
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
			update.Timezone = entity.Timezone;
			_context.VenueRepository.Update(update);
			await _context.SaveAsync();
		}

		private VenueDto MapToVenueDto(Venue from)
		{
			return new VenueDto
			{
				Address = from.Address,
				Description = from.Description,
				Id = from.Id,
				Name = from.Name,
				Phone = from.Phone,
				Timezone = from.Timezone,
				LayoutList = new List<LayoutDto>()
			};
		}

		private Venue MapToVenue(VenueDto from)
		{
			return new Venue
			{
				Address = from.Address,
				Description = from.Description,
				Name = from.Name,
				Phone = from.Phone,
				Timezone = from.Timezone
			};
		}

		private bool IsNameUnique(VenueDto entity, bool isCreating)
		{
			var data = from venues in _context.VenueRepository.GetList()
					   where venues.Name.Equals(entity.Name, StringComparison.OrdinalIgnoreCase)
					   select venues;

			return isCreating ?
				!data.Any() :
				!(from venues in data
				  where venues.Id != entity.Id
				  select venues).Any();
		}

		private Layout MapToLayout(LayoutDto from)
		{
			return new Layout
			{
				Description = from.Description,
				Id = from.Id,
				VenueId = from.VenueId
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
	}
}
