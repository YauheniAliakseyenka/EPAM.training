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
	internal class VenueService : IStoreService<VenueDto, int>
	{
		private readonly IWorkUnit _context;
		private readonly IStoreService<LayoutDto, int> _layoutService;

		public VenueService(IWorkUnit context, IStoreService<LayoutDto, int> layoutService)
		{
			_context = context;
			_layoutService = layoutService;
		}

		public void Create(VenueDto entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			if (IsNameNotUnique(entity.Name))
				throw new VenueException("Such venue already exists");

			if(entity.LayoutList == null || !entity.LayoutList.Any())
				throw new VenueException("Incorrect state of the venue. The venue must have at least one layout");

			var venueAdd = mapToVenue(entity);	
			using (var transaction = CustomTransactionScope.GetTransactionScope())
			{
				_context.VenueRepository.Create(venueAdd);
				_context.Save();
				entity.Id = venueAdd.Id;
				foreach (var layout in entity.LayoutList)
				{
					layout.VenueId = venueAdd.Id;
					_layoutService.Create(layout);
				}
				transaction.Complete();
			}
		}

		public void Delete(int id)
		{
			_context.VenueRepository.Delete(id);
			_context.Save();
		}

		public IEnumerable<VenueDto> FindBy(Expression<Func<VenueDto, bool>> expression)
		{
			var result = new List<VenueDto>();
			Expression<Func<Venue, bool>> predicate = x => expression.Compile().Invoke(mapToVenueDto(x));
			var list = _context.VenueRepository.FindBy(predicate).ToList();

			if (!list.Any())
				return result;

			list.ForEach(x =>
			{
				result.Add(mapToVenueDto(x));
			});

			return result;
		}

		public VenueDto Get(int id)
		{
			var venue = _context.VenueRepository.Get(id);

			if(venue == null)
				return null;

			return mapToVenueDto(venue);
		}

		public IEnumerable<VenueDto> GetList()
		{
			var temp = _context.VenueRepository.GetList();

			if (temp == null)
				return null;

			var result = new List<VenueDto>();

			temp.ToList().ForEach(x =>
			{
				result.Add(mapToVenueDto(x));
			});

			return result;
		}

        public void Update(VenueDto entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			if (IsNameNotUnique(entity.Name))
				throw new VenueException("Such venue already exists");

			var update = _context.VenueRepository.Get(entity.Id);
			update.Name = entity.Name;
			update.Phone = entity.Phone;
			update.Description = entity.Description;
			update.Address = entity.Address;
			_context.VenueRepository.Update(update);
			_context.Save();
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

		private bool IsNameNotUnique(string name)
		{
			return _context.VenueRepository.FindBy(y => y.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).Any();
		}
	}
}
