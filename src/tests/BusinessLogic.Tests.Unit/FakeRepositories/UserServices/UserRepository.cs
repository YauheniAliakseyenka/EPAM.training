using DataAccess.Entities;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogin.Unit.Tests.FakeRepositories
{
	internal class UserRepository : IRepository<User, string>
	{
		private List<User> _list;

		public UserRepository()
		{
			_list = new List<User>
			{
				new User { Id = "57b9433e-78ac-4619-91eb-dd7a5c130f08",
					PasswordHash ="AJIgHBUxxdjDCQnGdWpOduEHY3H9njwFR7xQiv+W/S71mnfHtnL+FakAla8OLctSPw==",
				Amount = 0M, UserName = "admin", Email ="admin_vasa@outlook.com", Culture = "ru",
					Timezone = TimeZoneInfo.Utc.Id},
				new User { Id = "c4195241-4621-4e52-95a1-714d2f005cbb",
					PasswordHash ="AL5sqo4EuA4QndxLBI76AP9qJhPKKsE4Z4BzPI9/dPfug71640OUJlF13BATwgq2ZA==",
				Amount = 1150.23M, UserName = "lena", Email ="lena@gmail.com", Culture = "ru",
					Timezone = TimeZoneInfo.Utc.Id},
				new User { Id = "c4195241-4241-se52-95a2-71dsa105cbaq",
					PasswordHash ="AL5sqo6EuA4QndxLsI76AP94JhPKKsE4Z4BzPI3/dPfug71640OUJlF13scTwgq2ZA==",
				Amount = 123.23M, UserName = "event_manager", Email ="ivanov@gmail.com", Culture = "be",
					Timezone = TimeZoneInfo.Utc.Id}
			};
		}

		public void Create(User entity)
		{
			_list.Add(entity);
		}

		public void Delete(string id)
		{
			_list.Remove(_list.FirstOrDefault(x => x.Id.Equals(id)));
		}

		public void DeleteBy(Expression<Func<User, bool>> expression)
		{
			_list.RemoveAll(new Predicate<User>(expression.Compile()));
		}

		public IQueryable<User> FindBy(Expression<Func<User, bool>> expression)
		{
			return _list.FindAll(new Predicate<User>(expression.Compile())).AsQueryable();
		}

		public User Get(string id)
		{
			return _list.FirstOrDefault(x => x.Id.Equals(id, StringComparison.Ordinal));
		}

		public IQueryable<User> GetList()
		{
			return _list.ToList().AsQueryable();
		}

		public void Update(User entity)
		{
			var update = _list.FirstOrDefault(x => x.Id.Equals(entity.Id));
			update.Surname = entity.Surname;
			update.Firstname = entity.Firstname;
			update.Culture = entity.Culture;
			update.Timezone = entity.Timezone;
			update.PasswordHash = entity.PasswordHash;
			update.Email = entity.Email;
			update.Amount = entity.Amount;
		}

        public Task<IEnumerable<User>> FindByAsync(Expression<Func<User, bool>> expression)
        {
            return Task.FromResult(_list.FindAll(new Predicate<User>(expression.Compile())).AsEnumerable());
        }

        public Task<User> GetAsync(string id)
        {
            return Task.FromResult(_list.FirstOrDefault(x => x.Id.Equals(id, StringComparison.Ordinal)));
        }

        public Task<IEnumerable<User>> GetListAsync()
        {
            return Task.FromResult(_list.AsEnumerable());
        }
    }
}
