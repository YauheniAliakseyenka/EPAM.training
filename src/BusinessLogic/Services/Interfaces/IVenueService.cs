using BusinessLogic.DTO;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface IVenueService : IStoreService<VenueDto, int>
    {
        Task<VenueDto> GetFullModel(int id);
    }
}
