using DataAccess;
using DataAccess.Entities;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BusinessLogic.Services
{
	public interface IStoreService<T, in Tkey>  where T : class, new()
	{
		void Create(T entity);
		void Update(T entity);
		void Delete(Tkey id);
		T Get(Tkey id);
		IEnumerable<T> FindBy(Expression<Func<T, bool>> expression);
		IEnumerable<T> GetList();
	}
}
