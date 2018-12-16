using BusinessLogic.Exceptions;
using BusinessLogic.Validators;
using BusinessLogic.ViewEntities;
using DataAccess;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
	internal class VenueService : LayoutService, IService<VenueView>
	{
		private IRepository<Venue> _venueRepo;

		public VenueService(IRepository<Venue> venueRepo, 
			IRepository<Layout> layoutRepo,
			IRepository<Area> areaRepo,
			IRepository<Seat> seatRepo) : base( layoutRepo, areaRepo, seatRepo)
		{
			_venueRepo = venueRepo;
		}

		public int Create(VenueView entity)
		{
			if (entity == null)
				throw new NullReferenceException();

			if (!VenueValidator.isNameUnique(entity.Name, GetList()))
				throw new VenueException("Such venue already exists");

			if(entity.LayoutList == null || entity.LayoutList.Count() == 0)
				throw new VenueException("Incorrect state of the venue. The venue must have at least one layout");

			var venueAdd = new Venue()
			{
				Address = entity.Address,
				Description = entity.Description,
				Name = entity.Name,
				Phone = entity.Phone,
				Id = entity.Id
			};
			
			using (var transaction = GetTransactionScope())
			{
				var id = _venueRepo.Create(venueAdd);
				foreach (var layout in entity.LayoutList)
				{
					layout.VenueId = id;
					base.Create(layout);
				}

				transaction.Complete();
			}

			return 1;
		}

		public new int Delete(int id)
		{
			return _venueRepo.Delete(id);
		}

		public IEnumerable<VenueView> Find(Expression<Func<VenueView, bool>> expression)
		{
			return GetList().Where(expression.Compile()).ToList();
		}

		public new VenueView Get(int id)
		{
			var venue = _venueRepo.Get(id);

			if(venue == null)
				return null;

			var result = new VenueView()
			{
				Address = venue.Address,
				Description = venue.Description,
				Name = venue.Name,
				Phone = venue.Phone,
				LayoutList = null,
				Id = venue.Id
			};

			return result;
		}

		public new IEnumerable<VenueView> GetList()
		{
			var temp = _venueRepo.GetList();

			if (temp == null)
				return null;

			var result = new List<VenueView>();

			temp.ToList().ForEach(x =>
			{
				result.Add(new VenueView()
				{
					Address = x.Address,
					Description = x.Description,
					Name = x.Name,
					Phone = x.Phone,
					LayoutList = null,
					Id = x.Id
				});
			});

			return result;
		}
		
		public void Update(VenueView entity)
		{
			if (entity == null)
				throw new NullReferenceException();

			if (VenueValidator.isNameUnique(entity.Name, GetList()))
				throw new VenueException("Such venue already exists");

			var update = new Venue()
			{
				Address = entity.Address,
				Description = entity.Description,
				Name = entity.Name,
				Phone = entity.Phone,
				Id = entity.Id
			};

			_venueRepo.Update(update);
		}
	}
}
