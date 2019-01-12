using DataAccess.Entities;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogin.Unit.Tests.FakeRepositories
{
	internal class PurchasedSeatRepository : IRepository<PurchasedSeat, int>
	{
		private List<PurchasedSeat> _list;

		public PurchasedSeatRepository()
		{
			_list = new List<PurchasedSeat>
			{
				new PurchasedSeat {SeatId = 15, OrderId = 1, Price = 45.7M },
				new PurchasedSeat {SeatId = 16, OrderId = 2, Price = 145.25M }
			};
		}

		public void Create(PurchasedSeat entity)
		{
			_list.Add(entity);
		}

		public void Delete(int id)
		{
			throw new NotImplementedException();
		}

		public void DeleteBy(Expression<Func<PurchasedSeat, bool>> expression)
		{
			_list.RemoveAll(new Predicate<PurchasedSeat>(expression.Compile()));
		}

		public IQueryable<PurchasedSeat> FindBy(Expression<Func<PurchasedSeat, bool>> expression)
		{
			return _list.FindAll(new Predicate<PurchasedSeat>(expression.Compile())).AsQueryable();
		}

		public PurchasedSeat Get(int id)
		{
			return _list.FirstOrDefault(x => x.SeatId == id);
		}

		public IQueryable<PurchasedSeat> GetList()
		{
			return _list.ToList().AsQueryable();
		}

		public void Update(PurchasedSeat entity)
		{
			throw new NotImplementedException();
		}

        public Task<IEnumerable<PurchasedSeat>> FindByAsync(Expression<Func<PurchasedSeat, bool>> expression)
        {
            return Task.FromResult(_list.FindAll(new Predicate<PurchasedSeat>(expression.Compile())).AsEnumerable());
        }

        public Task<PurchasedSeat> GetAsync(int id)
        {
            return Task.FromResult(_list.FirstOrDefault(x => x.SeatId == id));
        }

        public Task<IEnumerable<PurchasedSeat>> GetListAsync()
        {
            return Task.FromResult(_list.AsEnumerable());
        }
    }
}
