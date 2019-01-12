using BusinessLogic.BusinessModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
	public interface ICartService
	{
		Task AddSeat(int seatId, string userId);
		Task<IEnumerable<SeatModel>> GetOrderedSeats(string userId);
		Task DeleteUserCart(string userId);
		Task DeleteSeat(int seatId);
	}
}
