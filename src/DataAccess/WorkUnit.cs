using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using DataAccess.Entities;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataAccess
{
	internal class WorkUnit : IWorkUnit
	{
		public IRepository<OrderedSeat> OrderedSeatsRepository { get; private set; }
		public IRepository<Cart> CartRepository { get; private set; }
		public IRepository<EventSeat> EventSeatRepository { get; private set; }
		public IRepository<Event> EventRepository { get; private set; }
		public IRepository<EventArea> EventAreaRepository { get; private set; }
		public IRepository<Layout> LayoutRepository { get; private set; }
		public IRepository<Venue> VenueRepository { get; private set; }
		public IRepository<Order> OrderRepository { get; private set; }
		public IRepository<PurchasedSeat> PurchasedSeatRepository { get; private set; }
		public IRepository<User> UserRepository { get; private set; }
		public IRepository<Role> RoleRepository { get; private set; }
		public IRepository<UserRole> UserRoleRepository { get; private set; }
		public IRepository<Area> AreaRepository { get; private set; }
		public IRepository<Seat> SeatRepository { get; private set; }
        public IRepository<RefreshToken> RefreshTokenRepository { get; private set; }


        private readonly DataContext _context;
		private bool disposed = false;

		public WorkUnit(string connectionString)
		{
			_context = new DataContext(connectionString);
			OrderedSeatsRepository = new Repository<OrderedSeat>(_context);
			CartRepository = new Repository<Cart>(_context);
			EventSeatRepository = new Repository<EventSeat>(_context);
			EventRepository = new Repository<Event>(_context);
			EventAreaRepository = new Repository<EventArea>(_context);
			LayoutRepository = new Repository<Layout>(_context);
			VenueRepository = new Repository<Venue>(_context);
			OrderRepository = new Repository<Order>(_context);
			PurchasedSeatRepository = new Repository<PurchasedSeat>(_context);
			UserRepository = new Repository<User>(_context);
			RoleRepository = new Repository<Role>(_context);
			UserRoleRepository = new Repository<UserRole>(_context);
			AreaRepository = new Repository<Area>(_context);
			SeatRepository = new Repository<Seat>(_context);
            RefreshTokenRepository = new Repository<RefreshToken>(_context);
		}

		public IDbContextTransaction CreateTransaction()
		{
			return _context.Database.BeginTransaction();
		}

		public void Save()
		{
			_context.SaveChanges();
		}

		public async Task<int> SaveAsync()
		{
			return await _context.SaveChangesAsync();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
				return;

			if (disposing)
			{
                _context.Dispose();
            }

			disposed = true;
		}

        ~WorkUnit()
        {
            Dispose(false);
        }
	}
}
