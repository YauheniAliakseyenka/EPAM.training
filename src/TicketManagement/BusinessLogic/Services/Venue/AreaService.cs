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
using System.Transactions;

namespace BusinessLogic.Services
{
	internal class AreaService : SeatService, IService<AreaView>
	{
		private IRepository<Area> _areaRepo;

		public AreaService(IRepository<Area> areaRepo, IRepository<Seat> seatRepo) : base(seatRepo)
		{
			_areaRepo = areaRepo;
		}

		public int Create(AreaView entity)
		{
			if (entity == null)
				throw new NullReferenceException();

			if (entity.LayoutId == 0)
				throw new AreaException("Layout wasn't chosen");

			if(!AreaValidator.IsDescriptionUnique(entity.Description, Find(x=>x.LayoutId == entity.LayoutId)))
				throw new AreaException("Area description isnt' unique");

			if(entity.SeatList == null || entity.SeatList.Count() == 0)
				throw new AreaException("Incorrect state of the area. The area must have at least one seat");

			var areaAdd = new Area()
			{
				CoordX = entity.CoordX,
				CoordY = entity.CoordY,
				Description = entity.Description,
				LayoutId = entity.LayoutId,
				Id = entity.Id
			};

			using (var transaction = GetTransactionScope())
			{
				var id =_areaRepo.Create(areaAdd);
				foreach (var seat in entity.SeatList)
				{
					seat.AreaId = id;
					base.Create(seat);
				}

				transaction.Complete();
			}

			return 1;
		}

		public new int Delete(int id)
		{
			return _areaRepo.Delete(id);
		}

		public IEnumerable<AreaView> Find(Expression<Func<AreaView, bool>> expression)
		{
			return GetList().Where(expression.Compile()).ToList();
		}

		public new AreaView Get(int id)
		{
			var area = _areaRepo.Get(id);

			if (area == null)
				return null;

			var result = new AreaView()
			{
				CoordX = area.CoordX,
				CoordY =area.CoordY,
				Description = area.Description,
				LayoutId = area.LayoutId,
				SeatList = new List<SeatView>(),
				Id = area.Id
			};

			//result.SeatList.AddRange(_seatRepo.GetList().Where(x => x.AreaId == id).Select(x => x.Id).ToList());

			return result;
		}

		public new IEnumerable<AreaView> GetList()
		{
			var temp = _areaRepo.GetList();

			if (temp == null)
				return null;

			var result = new List<AreaView>();
			temp.ToList().ForEach(x =>
			{
				result.Add(new AreaView()
				{
					CoordX = x.CoordX,
					CoordY = x.CoordY,
					Description = x.Description,
					LayoutId = x.LayoutId,
					SeatList = null,
					Id = x.Id
				});
			});

			return result;
		}

		public void Update(AreaView entity)
		{
			if (entity == null)
				throw new NullReferenceException();

			if (entity.LayoutId == 0)
				throw new AreaException("Layout wasn't chosen");

			if (!AreaValidator.IsDescriptionUnique(entity.Description, Find(x => x.LayoutId == entity.LayoutId)))
				throw new AreaException("Area description isnt' unique");

			var update = new Area()
			{
				CoordX = entity.CoordX,
				CoordY = entity.CoordY,
				Description = entity.Description,
				LayoutId = entity.LayoutId,
				Id = entity.Id
			};

			_areaRepo.Update(update);
		}

		protected TransactionScope GetTransactionScope()
		{
			var transactionOptions = new TransactionOptions();
			transactionOptions.IsolationLevel = IsolationLevel.ReadCommitted;
			transactionOptions.Timeout = TransactionManager.MaximumTimeout;
			return new TransactionScope(TransactionScopeOption.Required, transactionOptions);
		}
	}
}
