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
	internal class OrderRepository : IRepository<Order, int>
	{
		private List<Order> _list;

		public OrderRepository()
		{
			_list = new List<Order>
			{
				new Order { Id = 1, Date =DateTime.Today.AddHours(2).AddMinutes(45),
				UserId = "57b9433e-78ac-4619-91eb-dd7a5c130f08" },
				new Order { Id = 2, Date =DateTime.Today.AddDays(2).AddHours(6).AddMinutes(45),
				UserId = "c4195241-4621-4e52-95a1-714d2f005cbb" },
			};
		}

		public void Create(Order entity)
		{
			_list.Add(entity);
		}

		public void Delete(int id)
		{
			_list.Remove(_list.Find(x => x.Id == id));
		}

		public void DeleteBy(Expression<Func<Order, bool>> expression)
		{
			_list.RemoveAll(new Predicate<Order>(expression.Compile()));
		}

		public IQueryable<Order> FindBy(Expression<Func<Order, bool>> expression)
		{
			return _list.FindAll(new Predicate<Order>(expression.Compile())).AsQueryable();
		}

		public Order Get(int id)
		{
			return _list.FirstOrDefault(x => x.Id == id);
		}

		public IQueryable<Order> GetList()
		{
			return _list.ToList().AsQueryable();
		}

		public void Update(Order entity)
		{
			throw new NotImplementedException();
		}
	}
}
