using System.Threading.Tasks;

namespace TicketManagementMVC.Infrastructure.BackgroundWorker
{
	public interface ISeatLocker
	{
		Task LockSeat(int seatId, string userId);
		Task UnlockSeat(int seatId);
	}
}