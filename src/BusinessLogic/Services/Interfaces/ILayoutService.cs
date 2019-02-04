using BusinessLogic.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
	public interface ILayoutService : IStoreService<LayoutDto, int>
	{
		Task<IEnumerable<LayoutDto>> GetLayoutsByVenue(int venueId);
	}
}
