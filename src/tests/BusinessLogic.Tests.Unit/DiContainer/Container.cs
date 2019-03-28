using Autofac;
using BusinessLogic.DiContainer;
using BusinessLogic.Tests.Unit.FakeRepositories;
using BusinessLogic.Tests.Unit.FakeRepositories.Data;
using DataAccess;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using System.Threading.Tasks;

namespace BusinessLogic.Tests.Unit.DiContainer
{
    internal class Container
    {
        public static IContainer GetContainer()
        {
            var builder = new ContainerBuilder();

			builder.RegisterInstance(GetUnit().Object).As<IWorkUnit>();
            builder.RegisterModule(new TestsModule());

            return builder.Build();
        }

        private static Mock<IWorkUnit> GetUnit()
        {
            var mockTransaction = new Mock<IDbContextTransaction>();
            mockTransaction.Setup(x => x.Commit()).Verifiable();
            mockTransaction.Setup(x => x.Rollback()).Verifiable();

            var workUnit = new Mock<IWorkUnit>();
			workUnit.SetupGet(x => x.VenueRepository).Returns(new FakeRepository<Venue>(FakeVenueData.Venues()));
			workUnit.SetupGet(x => x.LayoutRepository).Returns(new FakeRepository<Layout>(FakeVenueData.Layouts()));
			workUnit.SetupGet(x => x.AreaRepository).Returns(new FakeRepository<Area>(FakeVenueData.Areas()));
			workUnit.SetupGet(x => x.SeatRepository).Returns(new FakeRepository<Seat>(FakeVenueData.Seats()));
			workUnit.SetupGet(x => x.EventRepository).Returns(new FakeRepository<Event>(FakeEventData.Events()));
			workUnit.SetupGet(x => x.EventAreaRepository).Returns(new FakeRepository<EventArea>(FakeEventData.EventAreas()));
			workUnit.SetupGet(x => x.EventSeatRepository).Returns(new FakeRepository<EventSeat>(FakeEventData.EventSeats()));
			workUnit.SetupGet(x => x.UserRepository).Returns(new FakeRepository<User>(FakeUserData.Users()));
			workUnit.SetupGet(x => x.OrderedSeatsRepository).Returns(new FakeRepository<OrderedSeat>(FakeUserData.OrderedSeats()));
			workUnit.SetupGet(x => x.OrderRepository).Returns(new FakeRepository<Order>(FakeUserData.Orders()));
			workUnit.SetupGet(x => x.PurchasedSeatRepository).Returns(new FakeRepository<PurchasedSeat>(FakeUserData.PurchasedSeats()));
			workUnit.SetupGet(x => x.RoleRepository).Returns(new FakeRepository<Role>(FakeUserData.Roles()));
			workUnit.SetupGet(x => x.UserRoleRepository).Returns(new FakeRepository<UserRole>(FakeUserData.UserRoles()));
            workUnit.SetupGet(x => x.CartRepository).Returns(new FakeRepository<Cart>(FakeUserData.Carts()));
            workUnit.Setup(x => x.Save()).Verifiable();
			workUnit.Setup(x => x.SaveAsync()).Returns(Task.FromResult(0));
			workUnit.Setup(x => x.CreateTransaction()).Returns(mockTransaction.Object);

            return workUnit;
        }
    }
}
