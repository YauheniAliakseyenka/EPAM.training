using DataAccess.Entities;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace DataAccess
{
	public interface IWorkUnit : IDisposable
	{
		IRepository<OrderedSeat> OrderedSeatsRepository { get; }
		IRepository<Cart> CartRepository { get; }
		IRepository<EventSeat> EventSeatRepository { get; }
		IRepository<Event> EventRepository { get; }
		IRepository<EventArea> EventAreaRepository { get; }
		IRepository<Layout> LayoutRepository { get; }
		IRepository<Venue> VenueRepository { get; }
		IRepository<Order> OrderRepository { get; }
		IRepository<PurchasedSeat> PurchasedSeatRepository { get; }
		IRepository<User> UserRepository { get; }
		IRepository<Role> RoleRepository { get; }
		IRepository<UserRole> UserRoleRepository { get; }
		IRepository<Area> AreaRepository { get; }
		IRepository<Seat> SeatRepository { get; }
        IRepository<RefreshToken> RefreshTokenRepository { get; }

        void Save();
		Task<int> SaveAsync();
        IDbContextTransaction CreateTransaction();
	}
}
