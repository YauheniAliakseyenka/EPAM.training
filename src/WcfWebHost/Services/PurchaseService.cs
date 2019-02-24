using BusinessLogic.Exceptions;
using BusinessLogic.Services;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Permissions;
using System.ServiceModel;
using System.Threading.Tasks;
using WcfBusinessLogic.Core.Contracts.Data.BusinessModels;
using WcfBusinessLogic.Core.Contracts.Exceptions;
using WcfBusinessLogic.Core.Contracts.Services;
using WcfBusinessLogic.Core.Helpers.Parsers;
using WcfWebHost.Services.EmailService;
using WcfWebHost.Services.SeatLockerService;

namespace WcfWebHost.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    internal class WcfPurchaseService : IWcfPurchaseService
	{
		private readonly IOrderService _orderService;
		private readonly ISeatLocker _seatLocker;
		private readonly ICartService _cartService;
		private readonly IEmailService _emailService;

		public WcfPurchaseService(IOrderService orderService, 
			ISeatLocker seatLocker, 
			ICartService cartService, 
			IEmailService emailService)
		{
			_orderService = orderService;
			_seatLocker = seatLocker;
			_cartService = cartService;
			_emailService = emailService;
		}

        [PrincipalPermission(SecurityAction.Demand, Role = "Manager")]
        public async Task AddSeatToCart(int seatId, int userId)
		{
			try
			{
				await _seatLocker.LockSeat(seatId, userId);
			}
			catch (OrderException exception)
			{
				var fault = new ServiceValidationFaultDetails { Message = "Add seat to cart error" };
				throw new FaultException<ServiceValidationFaultDetails>(fault, exception.Message);
			}
		}
		
        [PrincipalPermission(SecurityAction.Demand, Role = "Manager")]
        public async Task<string> CancelOrderAndRefund(int orderId)
		{
			try
			{
				var amount = await _orderService.CancelOrderAndRefund(orderId);

				return amount.ToString(CultureInfo.InvariantCulture);
			}
			catch (OrderException exception)
			{
				var fault = new ServiceValidationFaultDetails { Message = "Cancel order error" };
				throw new FaultException<ServiceValidationFaultDetails>(fault, exception.Message);
			}
		}
		
        [PrincipalPermission(SecurityAction.Demand, Role = "Manager")]
        public async Task<string> CreateOrder(int userId)
		{
			try
			{
				_orderService.Ordered += _seatLocker.OrderCompleted;
				_orderService.Ordered += _emailService.Send;

				var amount = await _orderService.Create(userId);

				return amount.ToString(CultureInfo.InvariantCulture);
			}
			catch (OrderException exception)
			{
				var fault = new ServiceValidationFaultDetails { Message = "Create order error" };
				throw new FaultException<ServiceValidationFaultDetails>(fault, exception.Message);
			}
		}
		
        [PrincipalPermission(SecurityAction.Demand, Role = "Manager")]
        public async Task DeleteSeatFromCart(int seatId)
		{
			try
			{
				await _seatLocker.UnlockSeat(seatId);
			}
			catch (OrderException exception)
			{
				var fault = new ServiceValidationFaultDetails { Message = "Delete seat from cart error" };
				throw new FaultException<ServiceValidationFaultDetails>(fault, exception.Message);
			}
		}
		
        [PrincipalPermission(SecurityAction.Demand, Role = "Manager")]
        public async Task<IEnumerable<SeatModel>> GetOrderedSeats(int userId)
		{
			try
			{
				var result = await _cartService.GetOrderedSeats(userId);

				return result.Select(x => SeatModelBllToSeatModelContract(x));
			}
			catch (OrderException exception)
			{
				var fault = new ServiceValidationFaultDetails { Message = "Get ordered seats error" };
				throw new FaultException<ServiceValidationFaultDetails>(fault, exception.Message);
			}
		}
		
        [PrincipalPermission(SecurityAction.Demand, Role = "Manager")]
        public async Task<IEnumerable<OrderModel>> GetPurchaseHistory(int userId)
		{
			try
			{
				var result = await _orderService.GetPurchaseHistory(userId);

				return result.Select(x => OrderModelBllToOrderModelContract(x));
			}
			catch (OrderException exception)
			{
				var fault = new ServiceValidationFaultDetails { Message = "Create order error" };
				throw new FaultException<ServiceValidationFaultDetails>(fault, exception.Message);
			}
		}

        private OrderModel OrderModelBllToOrderModelContract(BusinessLogic.BusinessModels.OrderModel from)
		{
			return new OrderModel
			{
				PurchasedSeats = from.PurchasedSeats?.Select(x=>SeatModelBllToSeatModelContract(x)).ToList(),
				Order = OrderParser.ToOrderContract(from.Order)
			};
		}

		private SeatModel SeatModelBllToSeatModelContract(BusinessLogic.BusinessModels.SeatModel from)
		{
			return new SeatModel
			{
				Seat = EventSeatParser.ToEventSeatContract(from.Seat),
				Area = EventAreaParser.ToEventAreaContract(from.Area),
				Event = EventParser.ToEventContract(from.Event),
				Layout = LayoutParser.ToLayoutContract(from.Layout),
				Venue = VenueParser.ToVenueContract(from.Venue)
			};
		}
	}
}
