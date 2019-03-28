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
    internal class WcfVenueService : IWcfVenueService
    {
		private readonly IVenueService _venueService;

		public WcfVenueService(IVenueService venueService)
		{
			_venueService = venueService;
		}
		
		[PrincipalPermission(SecurityAction.Demand, Role = "Admin")]
		public async Task<int> Create(Venue entity)
        {
            try
            {
                var add = VenueParser.ToVenueDto(entity);
                await _venueService.Create(add);
                return add.Id;
            }
            catch (VenueException exception)
            {
                var fault = new ServiceValidationFaultDetails { Message = "Create venue error" };
                throw new FaultException<ServiceValidationFaultDetails>(fault, exception.Message);
            }
        }
		
		[PrincipalPermission(SecurityAction.Demand, Role = "Admin")]
		public async Task Delete(int id)
		{
            try
            {
                await _venueService.Delete(id);
            }
            catch (VenueException exception)
            {
                var fault = new ServiceValidationFaultDetails { Message = "Delete venue error" };
                throw new FaultException<ServiceValidationFaultDetails>(fault, exception.Message);
            }
        }

		[PrincipalPermission(SecurityAction.Demand, Role = "Manager")]
		[PrincipalPermission(SecurityAction.Demand, Role = "Admin")]
		public async Task<Venue> Get(int id)
		{
            try
            {
                var result = await _venueService.Get(id);

                return VenueParser.ToVenueContract(result);
            }
            catch (VenueException exception)
            {
                var fault = new ServiceValidationFaultDetails { Message = "Get venue error" };
                throw new FaultException<ServiceValidationFaultDetails>(fault, exception.Message);
            }
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Manager")]
        [PrincipalPermission(SecurityAction.Demand, Role = "Admin")]
        public async Task<Venue> GetFullModel(int id)
        {
            try
            {
                var result = await _venueService.GetFullModel(id);

                return VenueParser.ToVenueContract(result);
            }
            catch (VenueException exception)
            {
                var fault = new ServiceValidationFaultDetails { Message = "Get venue error" };
                throw new FaultException<ServiceValidationFaultDetails>(fault, exception.Message);
            }
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Manager")]
		[PrincipalPermission(SecurityAction.Demand, Role = "Admin")]
		public async Task<IEnumerable<Venue>> ToList()
		{
            try
            {
                var result = await _venueService.GetList();

                return result.Select(x => VenueParser.ToVenueContract(x));
            }
            catch (VenueException exception)
            {
                var fault = new ServiceValidationFaultDetails { Message = "Getting of list of venue error" };
                throw new FaultException<ServiceValidationFaultDetails>(fault, exception.Message);
            }
        }
		
		[PrincipalPermission(SecurityAction.Demand, Role = "Admin")]
		public async Task Update(Venue entity)
		{
            try
            {
                var update = VenueParser.ToVenueDto(entity);

                await _venueService.Update(update);
            }
            catch (VenueException exception)
            {
                var fault = new ServiceValidationFaultDetails { Message = "Update venue error" };
                throw new FaultException<ServiceValidationFaultDetails>(fault, exception.Message);
            }
        }
	}
}
