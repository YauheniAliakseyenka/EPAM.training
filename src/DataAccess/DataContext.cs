using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess
{
    internal class DataContext : DbContext
    {
        private readonly string _connectionString;

        public DataContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<Area> Areas { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventArea> EventAreas { get; set; }
        public DbSet<EventSeat> EventSeats { get; set; }
        public DbSet<Layout> Layouts { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Venue> Venues { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<PurchasedSeat> PurchasedSeats { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<OrderedSeat> OrderedSeats { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasKey(x => new { x.UserId, x.RoleId});
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(_connectionString);
        }

		public bool CreateEventStorePtocedure(object entity)
		{
			if (entity is Event eventEntity)
			{
				var output = new SqlParameter("@result", SqlDbType.Int) { Direction = ParameterDirection.Output };
				this.Database.ExecuteSqlCommand("EXEC AddEvent {0}, {1}, {2}, {3}, {4}, {5}, {6} OUTPUT",
					eventEntity.Title,
					eventEntity.Description,
					eventEntity.ImageURL,
					eventEntity.LayoutId,
					eventEntity.Date,
					eventEntity.CreatedBy,
					output);
				int id;
				if (int.TryParse(output.Value.ToString(), out id))
					eventEntity.Id = id;
				else
					throw new Exception("Add event result type exception");

				return true;
			}

			return false;
		}

		public bool UpdateEventStorePtocedure(object entity)
		{
			if (entity is Event eventEntity)
			{
				this.Database.ExecuteSqlCommand("EXEC UpdateEvent {0}, {1}, {2}, {3}, {4}, {5}, {6}",
					eventEntity.Title, 
					eventEntity.Description, 
					eventEntity.ImageURL, 
					eventEntity.LayoutId, 
					eventEntity.Date, 
					eventEntity.CreatedBy, 
					eventEntity.Id);

				return true;
			}

			return false;
		}
	}
}
