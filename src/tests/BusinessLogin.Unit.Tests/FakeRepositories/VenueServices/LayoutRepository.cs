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
	internal class LayoutRepository : IRepository<Layout, int>
	{
		private List<Layout> _list;

		public LayoutRepository()
		{
			_list = new List<Layout>
			{
				new Layout { Id = 1, VenueId = 1, Description = "The Hall 1" },
				new Layout { Id = 3, VenueId = 2, Description = "The Big hall, 1t floor" },
				new Layout { Id = 4, VenueId = 2, Description = "The Small hall, 1t floor" },
				new Layout { Id = 5, VenueId = 3, Description = "The Main hall" },
				new Layout { Id = 6, VenueId = 2, Description = "The Big hall, 2nd floor" },
				new Layout { Id = 7, VenueId = 1, Description = "The Main hall" }
			};
		}

		public void Create(Layout entity)
		{
			_list.Add(entity);
		}

		public void Delete(int id)
		{
			_list.Remove(_list.Find(x => x.Id == id));
		}

		public void DeleteBy(Expression<Func<Layout, bool>> expression)
		{
			_list.RemoveAll(new Predicate<Layout>(expression.Compile()));
		}

		public IQueryable<Layout> FindBy(Expression<Func<Layout, bool>> expression)
		{
			return _list.FindAll(new Predicate<Layout>(expression.Compile())).AsQueryable();
		}

		public Layout Get(int id)
		{
			return _list.FirstOrDefault(x => x.Id == id);
		}

		public IQueryable<Layout> GetList()
		{
			return _list.ToList().AsQueryable();
		}

		public void Update(Layout entity)
		{
			var update = _list.FirstOrDefault(x => x.Id == entity.Id);
			update.Description = entity.Description;
			update.VenueId = entity.VenueId;
		}
	}
}
