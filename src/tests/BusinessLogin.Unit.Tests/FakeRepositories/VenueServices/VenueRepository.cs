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
	internal class VenueRepository : IRepository<Venue, int>
	{
		private readonly List<Venue> _list;

		public VenueRepository()
		{
			_list = new List<Venue>
			{
				new Venue
				{
					Id = 1, Name ="Symphony Hall", Address = "Birmingham", Description = "Symphony Hall description", Phone = "111-111-111"
				},
				new Venue
				{
					Id = 2, Name ="ENO, Coliseum", Address = "London", Description = "ENO description", Phone = "222-222-222"
				},
				new Venue
				{
					Id = 3, Name ="Royal Albert Hall", Address = "London", Description = "Royal Albert Hall description", Phone = "333-333-333"
				}

			};
		}

		public void Create(Venue entity)
		{
			_list.Add(entity);
		}

		public void Delete(int id)
		{
			_list.Remove(_list.Find(x => x.Id == id));
		}

		public void DeleteBy(Expression<Func<Venue, bool>> expression)
		{
			_list.RemoveAll(new Predicate<Venue>(expression.Compile()));
		}

		public IQueryable<Venue> FindBy(Expression<Func<Venue, bool>> expression)
		{
			return _list.FindAll(new Predicate<Venue>(expression.Compile())).AsQueryable();
		}

		public Venue Get(int id)
		{
			return _list.FirstOrDefault(x => x.Id == id);
		}

		public IQueryable<Venue> GetList()
		{
			return _list.ToList().AsQueryable();
		}

		public void Update(Venue entity)
		{
			var update = _list.FirstOrDefault(x => x.Id == entity.Id);
			update.Name = entity.Name;
			update.Phone = entity.Phone;
			update.Description = entity.Description;
			update.Address = entity.Address;
		}
	}
}
