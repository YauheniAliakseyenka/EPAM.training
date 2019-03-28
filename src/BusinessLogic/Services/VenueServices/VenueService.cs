using BusinessLogic.DTO;
using BusinessLogic.Exceptions.VenueExceptions;
using BusinessLogic.Parsers;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
	internal class VenueService : IVenueService
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

            var venueAdd = VenueParser.MapToVenue(entity);
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
            if (IsHasEvents(id))
                throw new VenueException("Not allowed to delete. Venue has events setted up");

			var delete = await _context.VenueRepository.GetAsync(id);

			if (delete == null)
				return;

			_context.VenueRepository.Delete(delete);
			await _context.SaveAsync();
		}

		public async Task<VenueDto> Get(int id)
		{
			var venue = await _context.VenueRepository.GetAsync(id);

            return venue == null ? null : VenueParser.MapToVenueDto(venue);
		}

        public Task<VenueDto> GetFullModel(int id)
        {
            var data = (from venues in _context.VenueRepository.GetList()
                        join layotus in _context.LayoutRepository.GetList() on venues.Id equals layotus.VenueId
                        join areas in _context.AreaRepository.GetList() on layotus.Id equals areas.LayoutId
                        join seats in _context.SeatRepository.GetList() on areas.Id equals seats.AreaId
                        where venues.Id == id
                        select new { Venue = venues, Layout = layotus, Area = areas, Seat = seats }).ToList();

            if (!data.Any())
                return null;

            var venue = data.FirstOrDefault().Venue;
            var result = VenueParser.MapToVenueDto(venue);

            foreach(var row in data)
            {
				var layout = result.LayoutList.SingleOrDefault(x => x.Id == row.Layout.Id);
				//add layout if it isn't exist in a result
				if (layout is null)
                {
                    layout = LayoutParser.MapToLayoutDto(row.Layout);
                    result.LayoutList.Add(layout);
                }

				var area = layout.AreaList.SingleOrDefault(x => x.Id == row.Area.Id);
				//add area if it isn't exist in a result
				if (area is null)
                {
                    area = AreaParser.MapToAreaDto(row.Area);
                    layout.AreaList.Add(area);
                }

				area.SeatList.Add(SeatParser.MapToSeatDto(row.Seat));
            }

            return Task.FromResult(result);
        }

        public async Task<IEnumerable<VenueDto>> GetList()
		{
			var list = await _context.VenueRepository.GetListAsync();

			return list.Select(x => VenueParser.MapToVenueDto(x));
		}

        public async Task Update(VenueDto entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			if (!IsNameUnique(entity, false))
				throw new VenueException("Such venue already exists");

			if (entity.LayoutList == null || !entity.LayoutList.Any())
				throw new VenueException("Incorrect state of the venue. The venue must have at least one layout");

			using (var transaction = CustomTransactionScope.GetTransactionScope())
			{
				var update = await _context.VenueRepository.GetAsync(entity.Id);
				update.Name = entity.Name;
				update.Phone = entity.Phone;
				update.Description = entity.Description;
				update.Address = entity.Address;
				update.Timezone = entity.Timezone;
				_context.VenueRepository.Update(update);
				await _context.SaveAsync();

				var existingLayouts = await _context.LayoutRepository.FindByAsync(x => x.VenueId == entity.Id);

				//find and remove layouts which were deleted
				existingLayouts.Where(list2 => entity.LayoutList.All(list1 => list1.Id != list2.Id)).ToList()
					.ForEach(x =>
					{
						_context.LayoutRepository.Delete(x);
					});

				foreach(var layout in entity.LayoutList)
				{
					if (layout.Id == 0)
					{
						layout.VenueId = update.Id;
						await _layoutService.Create(layout);
					}
					else
						await _layoutService.Update(layout);
				}

				transaction.Complete();
			}	
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

        private bool IsHasEvents(int id)
        {
            var data = (from events in _context.EventRepository.GetList()
                        join layouts in _context.LayoutRepository.GetList() on events.LayoutId equals layouts.Id
                        join venues in _context.VenueRepository.GetList() on layouts.VenueId equals venues.Id
                        where venues.Id == id 
                        select events).ToList();

            return data.Any();
        }
    }
}
