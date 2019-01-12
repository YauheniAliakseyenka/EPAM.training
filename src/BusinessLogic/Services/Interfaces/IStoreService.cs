using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
	public interface IStoreService<T, in Tkey>  where T : class
	{
		Task Create(T entity);
		Task Update(T entity);
		Task Delete(Tkey id);
		Task<T> Get(Tkey id);
		Task<IEnumerable<T>> FindBy(Expression<Func<T, bool>> expression);
		Task<IEnumerable<T>> GetList();
	}
}
