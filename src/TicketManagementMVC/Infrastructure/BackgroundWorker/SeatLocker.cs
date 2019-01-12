using BusinessLogic.BusinessModels;
using BusinessLogic.Services;
using Hangfire;
using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace TicketManagementMVC.Infrastructure.BackgroundWorker
{
	internal class SeatLocker : ISeatLocker
	{
		private ICartService _cartService;
		private int _lockPeriod;

		public SeatLocker(ICartService cartService)
		{
			_cartService = cartService;

			if (!int.TryParse(ConfigurationManager.AppSettings["SeatLockTime"], out _lockPeriod))
				throw new Exception("Seat lock period is not valid");
		}

		public async Task LockSeat(int seatId, string userId)
		{
			await _cartService.AddSeat(seatId, userId);
			var date = DateTime.UtcNow.AddMinutes(_lockPeriod);
			var hour = date.TimeOfDay.Hours;
			var minute = date.TimeOfDay.Minutes;
			RecurringJob.AddOrUpdate<ISeatLocker>("unlockSeatId" + seatId, locker => locker.UnlockSeat(seatId), minute + " " + hour + " * * *");
		}

		public void OrderCompleted(object sender, OrderEventArgs args)
		{
			var seats = args.OrderModel.PurchasedSeats;

			if(seats.Any())
			{
				seats.ForEach(x =>
				{
					RecurringJob.RemoveIfExists("unlockSeatId" + x.Seat.Id);
				});
			}
		}

		public async Task UnlockSeat(int seatId)
		{
			await _cartService.DeleteSeat(seatId);
			RecurringJob.RemoveIfExists("unlockSeatId" + seatId);
		}
	}
}