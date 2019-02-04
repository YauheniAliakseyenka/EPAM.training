using BusinessLogic.BusinessModels;
using System.Threading.Tasks;

namespace TicketManagementMVC.Infrastructure.WebServices.Interfaces
{
	public interface ISeatLocker
	{
		Task LockSeat(int seatId, int userId);
		Task UnlockSeat(int seatId);
		void OrderCompleted(object sender, OrderEventArgs args);
	}
}