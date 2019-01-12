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
	internal class OrderedSeatsRepository : IRepository<OrderedSeat, int>
	{
		private List<OrderedSeat> _list;

		public OrderedSeatsRepository()
		{
			_list = new List<OrderedSeat>
			{
				new OrderedSeat {SeatId = 5,CartId = 1 },
				new OrderedSeat {SeatId = 13,CartId = 2 }
			};
		}

		public void Create(OrderedSeat entity)
		{
			_list.Add(entity);
		}

		public void Delete(int id)
		{
			_list.Remove(_list.Find(x => x.SeatId == id));
		}

		public void DeleteBy(Expression<Func<OrderedSeat, bool>> expression)
		{
			_list.RemoveAll(new Predicate<OrderedSeat>(expression.Compile()));
		}

		public IQueryable<OrderedSeat> FindBy(Expression<Func<OrderedSeat, bool>> expression)
		{
			return _list.FindAll(new Predicate<OrderedSeat>(expression.Compile())).AsQueryable();
		}

		public OrderedSeat Get(int id)
		{
			return _list.FirstOrDefault(x => x.SeatId == id);
		}

		public IQueryable<OrderedSeat> GetList()
		{
			return _list.ToList().AsQueryable();
		}

		public void Update(OrderedSeat entity)
		{
			throw new NotImplementedException();
		}

        public Task<IEnumerable<OrderedSeat>> FindByAsync(Expression<Func<OrderedSeat, bool>> expression)
        {
            return Task.FromResult(_list.FindAll(new Predicate<OrderedSeat>(expression.Compile())).AsEnumerable());
        }

        public Task<OrderedSeat> GetAsync(int id)
        {
            return Task.FromResult(_list.FirstOrDefault(x => x.SeatId == id));
        }

        public Task<IEnumerable<OrderedSeat>> GetListAsync()
        {
            return Task.FromResult(_list.AsEnumerable());
        }
    }
}
