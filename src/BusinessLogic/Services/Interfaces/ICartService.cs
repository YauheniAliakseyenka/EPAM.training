using BusinessLogic.BusinessModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
	public interface ICartService
	{
		Task AddSeat(int seatId, int userId);
		Task<IEnumerable<SeatModel>> GetOrderedSeats(int userId);
		Task DeleteUserCart(int userId);
		Task DeleteSeat(int seatId);
	}
}
