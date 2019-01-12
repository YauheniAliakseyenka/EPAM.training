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
	internal class CartRepository: IRepository<Cart, int>
	{
		private List<Cart> _list;

		public CartRepository()
		{
			_list = new List<Cart>
			{
				new Cart { Id = 1, UserId = "c4195241-4621-4e52-95a1-714d2f005cbb"},
				new Cart { Id = 2, UserId = "57b9433e-78ac-4619-91eb-dd7a5c130f08"},
			};
		}

		public void Create(Cart entity)
		{
			_list.Add(entity);
		}

		public void Delete(int id)
		{
			_list.Remove(_list.Find(x => x.Id == id));
		}

		public void DeleteBy(Expression<Func<Cart, bool>> expression)
		{
			_list.RemoveAll(new Predicate<Cart>(expression.Compile()));
		}

		public IQueryable<Cart> FindBy(Expression<Func<Cart, bool>> expression)
		{
			return _list.FindAll(new Predicate<Cart>(expression.Compile())).AsQueryable();
		}

		public Cart Get(int id)
		{
			return _list.FirstOrDefault(x => x.Id == id);
		}

		public IQueryable<Cart> GetList()
		{
			return _list.ToList().AsQueryable();
		}

		public void Update(Cart entity)
		{
			throw new NotImplementedException();
		}

        public Task<IEnumerable<Cart>> FindByAsync(Expression<Func<Cart, bool>> expression)
        {
            return Task.FromResult(_list.FindAll(new Predicate<Cart>(expression.Compile())).AsEnumerable());
        }

        public Task<Cart> GetAsync(int id)
        {
            return Task.FromResult(_list.FirstOrDefault(x => x.Id == id));
        }

        public Task<IEnumerable<Cart>> GetListAsync()
        {
            return Task.FromResult(_list.AsEnumerable());
        }
    }
}
