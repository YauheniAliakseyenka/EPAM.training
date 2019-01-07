using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface IRepository<T, in Tkey> where T: class, new()
    {
		void Create(T entity);
        void Update(T entity);
        void Delete(Tkey id);
		void DeleteBy(Expression<Func<T, bool>> expression);
		T Get(Tkey id);

		IQueryable<T> GetList();
        IQueryable<T> FindBy(Expression<Func<T, bool>> expression);
    }
}
