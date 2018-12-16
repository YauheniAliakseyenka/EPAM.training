using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IRepository<T> where T: class, new()
    {
		int Create(T entity);
        void Update(T entity);
        int Delete(int id);
        T Get(int id);
        IEnumerable<T> GetList();
	}
}
