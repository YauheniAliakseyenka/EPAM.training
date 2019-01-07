using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using DataAccess.Repositories;

namespace DataAccess
{
	internal class WorkUnit : IWorkUnit
	{
		public IRepository<OrderedSeat, int> OrderedSeatsRepository { get; private set; }
		public IRepository<Cart, int> CartRepository { get; private set; }
		public IRepository<EventSeat, int> EventSeatRepository { get; private set; }
		public IRepository<Event, int> EventRepository { get; private set; }
		public IRepository<EventArea, int> EventAreaRepository { get; private set; }
		public IRepository<Layout, int> LayoutRepository { get; private set; }
		public IRepository<Venue, int> VenueRepository { get; private set; }
		public IRepository<Order, int> OrderRepository { get; private set; }
		public IRepository<PurchasedSeat, int> PurchasedSeatRepository { get; private set; }
		public IRepository<User, string> UserRepository { get; private set; }
		public IRepository<Role, int> RoleRepository { get; private set; }
		public IRepository<UserRole, int> UserRoleRepository { get; private set; }
		public IRepository<Area, int> AreaRepository { get; private set; }
		public IRepository<Seat, int> SeatRepository { get; private set; }

		private readonly DataContext _context;

		public WorkUnit(string connectionString)
		{
			_context = new DataContext(connectionString);
			OrderedSeatsRepository = new Repository<OrderedSeat, int>(_context);
			CartRepository = new Repository<Cart, int>(_context);
			EventSeatRepository = new Repository<EventSeat, int>(_context);
			EventRepository = new Repository<Event, int>(_context);
			EventAreaRepository = new Repository<EventArea, int>(_context);
			LayoutRepository = new Repository<Layout, int>(_context);
			VenueRepository = new Repository<Venue, int>(_context);
			OrderRepository = new Repository<Order, int>(_context);
			PurchasedSeatRepository = new Repository<PurchasedSeat, int>(_context);
			UserRepository = new Repository<User, string>(_context);
			RoleRepository = new Repository<Role, int>(_context);
			UserRoleRepository = new Repository<UserRole, int>(_context);
			AreaRepository = new Repository<Area, int>(_context);
			SeatRepository = new Repository<Seat, int>(_context);
		}

		public DbTransaction CreateTransaction()
		{
			return _context.Database.BeginTransaction().UnderlyingTransaction;
		}

		public void Save()
		{
			_context.SaveChanges();
		}

		public void SaveAsync()
		{
			_context.SaveChangesAsync();
		}
	}
}
