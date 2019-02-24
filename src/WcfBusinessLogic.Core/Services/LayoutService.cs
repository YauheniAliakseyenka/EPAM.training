using BusinessLogic.Exceptions.VenueExceptions;
using BusinessLogic.Services;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.ServiceModel;
using System.Threading.Tasks;
using WcfBusinessLogic.Core.Contracts.Data;
using WcfBusinessLogic.Core.Contracts.Exceptions;
using WcfBusinessLogic.Core.Contracts.Services;
using WcfBusinessLogic.Core.Helpers.Parsers;

namespace WcfBusinessLogic.Core.Services
{
	internal class WcfLayoutService : IWcfLayoutService
	{
		private ILayoutService _layoutService;

		public WcfLayoutService(ILayoutService layoutService)
		{
			_layoutService = layoutService;
		}

		[PrincipalPermission(SecurityAction.Demand, Role = "Manager")]
		[PrincipalPermission(SecurityAction.Demand, Role = "Admin")]
		public async Task<IEnumerable<Layout>> GetLayoutsByVenue(int venueId)
		{
            try
            {
                var result = await _layoutService.GetLayoutsByVenue(venueId);

                return result.Select(x => LayoutParser.ToLayoutContract(x));
            }
            catch (LayoutException exception)
            {
                var fault = new ServiceValidationFaultDetails { Message = "Getting of list of layouts by venue error" };
                throw new FaultException<ServiceValidationFaultDetails>(fault, exception.Message);
            }
        }

		[PrincipalPermission(SecurityAction.Demand, Role = "Manager")]
		[PrincipalPermission(SecurityAction.Demand, Role = "Admin")]
		public async Task<IEnumerable<Layout>> ToList()
		{
            try
            {
                var result = await _layoutService.GetList();

                return result.Select(x => LayoutParser.ToLayoutContract(x));
            }
            catch (LayoutException exception)
            {
                var fault = new ServiceValidationFaultDetails { Message = "Getting of list of layouts error" };
                throw new FaultException<ServiceValidationFaultDetails>(fault, exception.Message);
            }
        }
	}
}
