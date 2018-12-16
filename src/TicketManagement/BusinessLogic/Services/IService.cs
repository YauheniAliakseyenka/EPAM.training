using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
	public interface IService<T> : IRepository<T> where T : class, new()
	{
		IEnumerable<T> Find(Expression<Func<T, bool>> expression);
	}
}
