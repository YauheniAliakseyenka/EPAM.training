using Autofac;
using BusinessLogic.DTO;
using BusinessLogic.Services;
using BusinessLogic.Services.EventServices;
using BusinessLogic.Services.UserServices;

namespace BusinessLogic.DiContainer
{
    internal class TestsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EventService>().As<IEventService>();
            builder.RegisterType<EventAreaService>().As<IStoreService<EventAreaDto, int>>();
            builder.RegisterType<EventSeatService>().As<IStoreService<EventSeatDto, int>>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<VenueService>().As<IStoreService<VenueDto, int>>();
            builder.RegisterType<LayoutService>().As<ILayoutService>();
            builder.RegisterType<AreaService>().As<IStoreService<AreaDto, int>>();
            builder.RegisterType<SeatService>().As<IStoreService<SeatDto, int>>();
            builder.RegisterType<CartService>().As<ICartService>();
            builder.RegisterType<OrderService>().As<IOrderService>();
        }

    }
}
