using Autofac;
using BusinessLogic.DTO;
using BusinessLogic.Services;
using BusinessLogic.Services.EventServices;
using BusinessLogic.Services.UserServices;
using DataAccess;

namespace BusinessLogic.DiContainer
{
    internal class WebModule : Module
    {
        public string ConnectionString { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WorkUnit>().As<IWorkUnit>().WithParameter("connectionString", ConnectionString).InstancePerLifetimeScope();
            
            //services
            builder.RegisterType<EventService>().As<IEventService>().InstancePerLifetimeScope();
            builder.RegisterType<EventAreaService>().As<IStoreService<EventAreaDto, int>>().InstancePerLifetimeScope();
            builder.RegisterType<EventSeatService>().As<IStoreService<EventSeatDto, int>>().InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<VenueService>().As<IStoreService<VenueDto, int>>().InstancePerLifetimeScope();
            builder.RegisterType<LayoutService>().As<ILayoutService>().InstancePerLifetimeScope();
            builder.RegisterType<AreaService>().As<IStoreService<AreaDto, int>>().InstancePerLifetimeScope();
            builder.RegisterType<SeatService>().As<IStoreService<SeatDto, int>>().InstancePerLifetimeScope();
            builder.RegisterType<CartService>().As<ICartService>().InstancePerLifetimeScope();
            builder.RegisterType<OrderService>().As<IOrderService>().InstancePerLifetimeScope();
        }
    }
}
