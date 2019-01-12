using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
	internal class Repository<T,  Tkey> : IRepository<T, Tkey> where T : class, new()
	{
        private DbSet<T> _dbSet;
		private DataContext _context;

        public Repository(DataContext context)
		{
			_context = context;
			_dbSet = context.Set<T>();
		}

        public void Create(T entity)
        {
			_dbSet.Add(entity);
        }

		public void Delete(Tkey id)
		{
            var entity = _dbSet.Find(id);
			_dbSet.Remove(entity);
		}

		public T Get(Tkey id)
		{
            return _dbSet.Find(id);
		}

		public void Update(T entity)
		{
			_context.Entry(entity).State = EntityState.Modified;
		}
        
		public IQueryable<T> GetList()
		{
            return _dbSet;
		}

		public IQueryable<T> FindBy(Expression<Func<T, bool>> expression)
		{
			return _dbSet.Where(expression.Compile()).AsQueryable();
		}

		public void DeleteBy(Expression<Func<T, bool>> expression)
		{
			_dbSet.RemoveRange(_dbSet.Where(expression.Compile()));
		}

		public async Task<T> GetAsync(Tkey id)
		{
			return await _dbSet.FindAsync(id);
		}

		public async Task<IEnumerable<T>> GetListAsync()
		{
			return await _dbSet.ToListAsync();
		}
	}
}
