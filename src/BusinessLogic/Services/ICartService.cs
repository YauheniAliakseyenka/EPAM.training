using BusinessLogic.BusinessModels;
using BusinessLogic.DTO;
using System.Collections.Generic;

namespace BusinessLogic.Services
{
    public interface ICartService
    {
        void AddSeat(int seatId, string userId);
        IEnumerable<SeatModel> GetOrderedSeats(string userId);
        void DeleteUserCart(string userId);
		void UnlockSeat(int seatId);
	}
}
