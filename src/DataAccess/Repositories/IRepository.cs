using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface IRepository<T, in Tkey> where T: class
    {
		void Create(T entity);
        void Update(T entity);
        void Delete(Tkey id);
		void DeleteBy(Expression<Func<T, bool>> expression);

		T Get(Tkey id);
		Task<T> GetAsync(Tkey id);

		IQueryable<T> GetList();
		Task<IEnumerable<T>> GetListAsync();

		IQueryable<T> FindBy(Expression<Func<T, bool>> expression);
	}
}
