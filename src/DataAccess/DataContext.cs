using System.Data.Entity;
using DataAccess.Entities;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DataAccess
{
    internal class DataContext : DbContext
    {
        public DataContext(string connectionStringName): base(connectionStringName)
        {
            Database.SetInitializer<DataContext>(null);
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //setting of stored procedures for event
            modelBuilder.Entity<Event>().
                MapToStoredProcedures(x=>
                x.Insert(sp=>sp.HasName("AddEvent")
                    .Parameter(p=>p.Title, "Title")
                    .Parameter(p=>p.LayoutId, "LayoutId")
                    .Parameter(p => p.Description, "Description")
					.Parameter(p=>p.ImageURL, "ImageURL")
                    .Parameter(p => p.Date, "Date")
					.Parameter(p=>p.CreatedBy, "CreatedBy"))
               .Delete(sp=>sp.HasName("DeleteEvent")
                    .Parameter(p=>p.Id,"Id"))
               .Update(sp=>sp.HasName("UpdateEvent")
                    .Parameter(p=>p.Title, "Title")
                    .Parameter(p => p.LayoutId, "LayoutId")
                    .Parameter(p => p.Description, "Description")
					.Parameter(p => p.ImageURL, "ImageURL")
					.Parameter(p => p.Date, "Date")
					.Parameter(p=>p.CreatedBy, "CreatedBy")
                    .Parameter(p=>p.Id, "Id")));
		}
    }
}
