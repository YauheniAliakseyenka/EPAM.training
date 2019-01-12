using Autofac;
using BusinessLogic.DiContainer;
using BusinessLogin.Unit.Tests.FakeRepositories;
using DataAccess;
using Moq;
using System.Data.Common;

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
            var mockTransaction = new Mock<DbTransaction>();
            mockTransaction.Setup(x => x.Commit()).Verifiable();
            mockTransaction.Setup(x => x.Rollback()).Verifiable();

            var workUnit = new Mock<IWorkUnit>();
            workUnit.SetupGet(x => x.VenueRepository).Returns(new VenueRepository());
            workUnit.SetupGet(x => x.LayoutRepository).Returns(new LayoutRepository());
            workUnit.SetupGet(x => x.AreaRepository).Returns(new AreaRepository());
            workUnit.SetupGet(x => x.SeatRepository).Returns(new SeatRepository());
            workUnit.SetupGet(x => x.EventRepository).Returns(new EventRepository());
            workUnit.SetupGet(x => x.EventAreaRepository).Returns(new EventAreaRepository());
            workUnit.SetupGet(x => x.EventSeatRepository).Returns(new EventSeatRepository());
            workUnit.SetupGet(x => x.UserRepository).Returns(new UserRepository());
            workUnit.SetupGet(x => x.OrderedSeatsRepository).Returns(new OrderedSeatsRepository());
            workUnit.SetupGet(x => x.OrderRepository).Returns(new OrderRepository());
            workUnit.SetupGet(x => x.PurchasedSeatRepository).Returns(new PurchasedSeatRepository());
            workUnit.SetupGet(x => x.RoleRepository).Returns(new RoleRepostiroy());
            workUnit.SetupGet(x => x.UserRoleRepository).Returns(new UserRoleRepository());
            workUnit.SetupGet(x => x.CartRepository).Returns(new CartRepository());
            workUnit.Setup(x => x.Save()).Verifiable();
            workUnit.Setup(x => x.CreateTransaction()).Returns(mockTransaction.Object);

            return workUnit;
        }
    }
}
