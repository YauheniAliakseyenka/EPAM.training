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
	internal class EventRepository : IRepository<Event, int>
	{
		private List<Event> _list;

		public EventRepository()
		{
			_list = new List<Event>
			{
				new Event{ Id = 1, LayoutId = 1, Date = DateTime.Today.AddMonths(1).AddHours(15).AddMinutes(30),
					Description = "The highpoint of Andris Nelsons’ final season as the CBSO’s music director – a concert performance of Wagner’s final music drama of almost alarming maturity",
					Title ="Parsifal", ImageURL="http://localhost:61962/Content/images/uploads/1.jpg"},
				new Event{ Id = 4, LayoutId = 1, Date = DateTime.Today.AddMonths(3).AddHours(20).AddMinutes(15),
					Description = "Conducted with perfect spaciousness by Edward Gardner, Richard Jones’s fabulous detailed and humane production of Wagner’s great comedy ",
					Title ="The Mastersingers of Nuremberg", ImageURL="http://localhost:61962/Content/images/uploads/4.jpg"},
				new Event{ Id = 2, LayoutId = 3, Date = DateTime.Today.AddMonths(2).AddHours(20),
					Description = "Conducted with perfect spaciousness by Edward Gardner, Richard Jones’s fabulous detailed and humane production of Wagner’s great comedy ",
					Title ="The Mastersingers of Nuremberg", ImageURL="http://localhost:61962/Content/images/uploads/4.jpg"},
				new Event{ Id = 3, LayoutId = 5, Date = DateTime.Today.AddMonths(1).AddHours(19).AddMinutes(30),
					Description = "Solo-violin sonatas and partitas from Alina Ibragimova", Title ="Late-night Bach: Proms",
				ImageURL="http://localhost:61962/Content/images/uploads/2.jpg" }
			};
		}

		public void Create(Event entity)
		{
			_list.Add(entity);
		}

		public void Delete(int id)
		{
			_list.Remove(_list.Find(x => x.Id == id));
		}

		public void DeleteBy(Expression<Func<Event, bool>> expression)
		{
			_list.RemoveAll(new Predicate<Event>(expression.Compile()));
		}

		public IQueryable<Event> FindBy(Expression<Func<Event, bool>> expression)
		{
			return _list.FindAll(new Predicate<Event>(expression.Compile())).AsQueryable();
		}

        public Event Get(int id)
		{
			return _list.FirstOrDefault(x => x.Id == id);
		}

        public IQueryable<Event> GetList()
        {
            return _list.AsQueryable();
        }

        public Task<IEnumerable<Event>> FindByAsync(Expression<Func<Event, bool>> expression)
        {
            return Task.FromResult(_list.FindAll(new Predicate<Event>(expression.Compile())).AsEnumerable());
        }

        public Task<Event> GetAsync(int id)
        {
            return Task.FromResult(_list.FirstOrDefault(x => x.Id == id));
        }     

        public Task<IEnumerable<Event>> GetListAsync()
        {
            return Task.FromResult(_list.AsEnumerable());
        }

        public void Update(Event entity)
		{
			var update = _list.FirstOrDefault(x => x.Id == entity.Id);
			update.Description = entity.Description;
			update.Title = entity.Title;
			update.LayoutId = entity.LayoutId;
			update.ImageURL = entity.ImageURL;
			update.Date = entity.Date;
			update.CreatedBy = entity.CreatedBy;
		}
	}
}
