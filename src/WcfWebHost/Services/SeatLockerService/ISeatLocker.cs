using BusinessLogic.BusinessModels;
using System.Threading.Tasks;

namespace WcfWebHost.Services.SeatLockerService
{
	public interface ISeatLocker
	{
		Task LockSeat(int seatId, int userId);
		Task UnlockSeat(int seatId);
		void OrderCompleted(object sender, OrderEventArgs args);
	}
}