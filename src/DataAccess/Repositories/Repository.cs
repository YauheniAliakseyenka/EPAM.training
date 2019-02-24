using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
	internal class Repository<T> : IRepository<T> where T : class
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
			if (_context.CreateEventStorePtocedure(entity))
				return;

			_dbSet.Add(entity);
		}

		public void Delete(T entity)
		{
			_dbSet.Remove(entity);
		}
     
		public T Get(params object[] keys)
		{
            return _dbSet.Find(keys);
		}
      
		public void Update(T entity)
		{
			if (_context.UpdateEventStorePtocedure(entity))
				return;

			if (_context.Entry(entity).State == EntityState.Detached)
				_dbSet.Attach(entity);

			_context.Entry(entity).State = EntityState.Modified;
		}
        
		public IQueryable<T> GetList()
		{
            return _dbSet;
		}
       
		public Task<IEnumerable<T>> FindByAsync(Func<T, bool> expression)
		{
			return Task.FromResult(_dbSet.Where(expression));
		}

		public async Task<T> GetAsync(params object[] keys)
		{
			return await _dbSet.FindAsync(keys);
		}

		public async Task<IEnumerable<T>> GetListAsync()
		{
			return await _dbSet.ToListAsync();
		}
	}
}
