using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
	public interface IStoreService<T, in Tkey>  where T : class
	{
		Task Create(T entity);
		Task Update(T entity);
		Task Delete(Tkey id);
		Task<T> Get(Tkey id);
		Task<IEnumerable<T>> GetList();
	}
}
