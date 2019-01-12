using DataAccess.Entities;
using DataAccess.Repositories;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace DataAccess
{
	public interface IWorkUnit : IDisposable
	{
		IRepository<OrderedSeat, int> OrderedSeatsRepository { get; }
		IRepository<Cart, int> CartRepository { get; }
		IRepository<EventSeat, int> EventSeatRepository { get; }
		IRepository<Event, int> EventRepository { get; }
		IRepository<EventArea, int> EventAreaRepository { get; }
		IRepository<Layout, int> LayoutRepository { get; }
		IRepository<Venue, int> VenueRepository { get; }
		IRepository<Order, int> OrderRepository { get; }
		IRepository<PurchasedSeat, int> PurchasedSeatRepository { get; }
		IRepository<User, string> UserRepository { get; }
		IRepository<Role, int> RoleRepository { get; }
		IRepository<UserRole, int> UserRoleRepository { get; }
		IRepository<Area, int> AreaRepository { get; }
		IRepository<Seat, int> SeatRepository { get; }

		void Save();
		Task<int> SaveAsync();
		DbTransaction CreateTransaction();
	}
}
