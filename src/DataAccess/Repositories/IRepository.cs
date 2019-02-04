using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface IRepository<T> where T: class
    {
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);

        /// <summary>
        /// Find an entity by key values
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        T Get(params object[] keys);

        /// <summary>
        /// Find an entity by key values async
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<T> GetAsync(params object[] keys);

        IQueryable<T> GetList();
        Task<IEnumerable<T>> GetListAsync();

		Task<IEnumerable<T>> FindByAsync(Func<T, bool> expression);
	}
}
