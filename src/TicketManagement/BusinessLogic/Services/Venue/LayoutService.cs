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
	internal class LayoutService : AreaService, IService<LayoutView> 
	{
		private IRepository<Layout> _layoutRepo;

		public LayoutService(IRepository<Layout> layoutRepo, 
			IRepository<Area> areaRepo,
			IRepository<Seat> seatRepo) : base(areaRepo, seatRepo)
		{
			_layoutRepo = layoutRepo;
		}

		public int Create(LayoutView entity)
		{
			if (entity == null)
				throw new NullReferenceException();

			if (entity.VenueId == 0)
				throw new LayoutException("Venue wasn't chosen");

			if (!LayoutValidator.IsDescriptionUnique(entity.Description, Find(x => x.VenueId == entity.VenueId)))
				throw new LayoutException("Layout description isnt' unique");

			if (entity.AreaList == null || entity.AreaList.Count() == 0)
				throw new LayoutException("Incorrect state of the layout. The layout must have at least one area");

			var layoutAdd = new Layout()
			{
				Description = entity.Description,
				VenueId = entity.VenueId,
				Id = entity.Id
			};

			using (var transaction = GetTransactionScope())
			{
				var id = _layoutRepo.Create(layoutAdd);
				foreach (var area in entity.AreaList)
				{
					area.LayoutId = id;
					base.Create(area);
				}

				transaction.Complete();
			}

			return 1;
		}

		public new int Delete(int id)
		{
			return _layoutRepo.Delete(id);
		}

		public IEnumerable<LayoutView> Find(Expression<Func<LayoutView, bool>> expression)
		{
			return GetList().Where(expression.Compile()).ToList();
		}

		public new LayoutView Get(int id)
		{
			var layout = _layoutRepo.Get(id);

			if (layout == null)
				return null;

			var result = new LayoutView()
			{
				Description = layout.Description,
				VenueId = layout.VenueId,
				Id = layout.Id,
				AreaList = null
			};
			return result;
		}

		public new  IEnumerable<LayoutView> GetList()
		{
			var temp = _layoutRepo.GetList();

			if (temp == null)
				return null;

			var result = new List<LayoutView>();

			temp.ToList().ForEach(x =>
			{
				result.Add(new LayoutView()
				{
					Description = x.Description,
					VenueId = x.VenueId,
					Id = x.Id,
					AreaList = null
				});
			});

			return result;
		}

		public void Update(LayoutView entity)
		{
			if (entity == null)
				throw new NullReferenceException();

			if (entity.VenueId == 0)
				throw new LayoutException("Venue wasn't chosen");

			if (!LayoutValidator.IsDescriptionUnique(entity.Description, Find(x => x.VenueId == entity.VenueId)))
				throw new LayoutException("Area description isnt' unique");

			var update = new Layout()
			{
				Description = entity.Description,
				VenueId = entity.VenueId,
				Id = entity.Id
			};

			_layoutRepo.Update(update);
		}
	}
}
