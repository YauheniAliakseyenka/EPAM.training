using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Tests.Unit.FakeRepositories
{
	internal class FakeRepository<T> : IRepository<T> where T : class
	{
		private List<T> _store;

		public FakeRepository(List<T> store)
		{
			_store = store;
		}

		public void Create(T entity)
		{
			_store.Add(entity);
			IncrementId(entity);
		}

		public void Delete(T entity)
		{
			_store.Remove(entity);
		}

		public Task<IEnumerable<T>> FindByAsync(Func<T, bool> expression)
		{
			return Task.FromResult(_store.FindAll(new Predicate<T>(expression)).AsEnumerable());
		}

		public T Get(params object[] keys)
		{
			return GetEntityByKeys(keys);
		}

		public Task<T> GetAsync(params object[] keys)
		{
			return Task.FromResult(GetEntityByKeys(keys));
		}

		public IQueryable<T> GetList()
		{
			return _store.AsQueryable();
		}

		public Task<IEnumerable<T>> GetListAsync()
		{
			return Task.FromResult(_store.AsEnumerable());
		}

		//a service will update a value in a list in any case because of object type, so we don't need to implement "update"
		public void Update(T entity)
		{
			return;
		}

		private T GetEntityByKeys(params object[] keys)
		{
			if (keys == null)
				return null;

			var properties = typeof(T).GetProperties();

			//get all key properties and order them by named parameter "Order" defined in ColumnAttribute
			var idProperties = properties.Where(x => x.GetCustomAttributes(false).Any(y => y.GetType() == typeof(KeyAttribute))). 
				OrderBy(x => (x.GetCustomAttributes(typeof(ColumnAttribute), false).FirstOrDefault() as ColumnAttribute).Order).ToList();

			if (keys.Count() != idProperties.Count)
				return null;

			return _store.Find(x =>
			{
				bool isFound = true;
				for (int i = 0; i < idProperties.Count; i++)
				{
					if (!idProperties[i].GetValue(x).Equals(keys[i]))
					{
						isFound = false;
						break;
					}
				}

				return isFound;
			});
		}

		private void IncrementId(T entity)
		{
			var property = typeof(T).GetProperties().FirstOrDefault(x =>
			{
				var attribute = x.GetCustomAttributes(false).SingleOrDefault(y => y.GetType() == typeof(KeyAttribute));

				if (attribute is null)
					return false;

				attribute = x.GetCustomAttributes(false).SingleOrDefault(y => y.GetType() == typeof(DatabaseGeneratedAttribute));

				if (attribute is null)
					return true;

				var attributeDatabaseGenerated = attribute as DatabaseGeneratedAttribute;
				if (attributeDatabaseGenerated.DatabaseGeneratedOption != DatabaseGeneratedOption.None)
					return true;

				return false;
			});
			property?.SetValue(entity, _store.Count + 1);
		}
	}
}
