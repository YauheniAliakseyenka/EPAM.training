using BusinessLogic.DTO;
using BusinessLogic.Exceptions.EventExceptions;
using BusinessLogic.Services;
using System.Security.Permissions;
using System.ServiceModel;
using System.Threading.Tasks;
using WcfBusinessLogic.Core.Contracts.Data;
using WcfBusinessLogic.Core.Contracts.Exceptions;
using WcfBusinessLogic.Core.Contracts.Services;
using WcfBusinessLogic.Core.Helpers.Parsers;

namespace WcfBusinessLogic.Core.Services
{
    internal class WcfEventAreaService : IWcfEventAreaService
    {
        private IStoreService<EventAreaDto, int> _eventAreaService;

        public WcfEventAreaService(IStoreService<EventAreaDto, int> eventAreaService)
        {
            _eventAreaService = eventAreaService;
        }

		[PrincipalPermission(SecurityAction.Demand, Role = "Manager")]
		public async Task<int> Create(EventArea entity)
		{
			try
			{
				var add = EventAreaParser.ToEventAreaDto(entity);
				await _eventAreaService.Create(add);

				return add.Id;
			}
			catch (EventSeatException exception)
			{
				var fault = new ServiceValidationFaultDetails { Message = "Create event seat error" };
				throw new FaultException<ServiceValidationFaultDetails>(fault, exception.Message);
			}
			catch (EventAreaException exception)
			{
				var fault = new ServiceValidationFaultDetails { Message = "Create event area error" };
				throw new FaultException<ServiceValidationFaultDetails>(fault, exception.Message);
			}
		}

		[PrincipalPermission(SecurityAction.Demand, Role = "Manager")]
		public async Task Delete(int id)
		{
			try
			{
				await _eventAreaService.Delete(id);
			}
			catch (EventSeatException exception)
			{
				var fault = new ServiceValidationFaultDetails { Message = "Delete event seat error" };
				throw new FaultException<ServiceValidationFaultDetails>(fault, exception.Message);
			}
			catch (EventAreaException exception)
			{
				var fault = new ServiceValidationFaultDetails { Message = "Delete event area error" };
				throw new FaultException<ServiceValidationFaultDetails>(fault, exception.Message);
			}
		}

		[PrincipalPermission(SecurityAction.Demand, Role = "Manager")]
		public async Task<EventArea> Get(int id)
		{
			try
			{
				var result = await _eventAreaService.Get(id);

				return EventAreaParser.ToEventAreaContract(result);
			}
			catch (EventAreaException exception)
			{
				var fault = new ServiceValidationFaultDetails { Message = "Get event area error" };
				throw new FaultException<ServiceValidationFaultDetails>(fault, exception.Message);
			}
		}

		[PrincipalPermission(SecurityAction.Demand, Role = "Manager")]
		public async Task Update(EventArea entity)
		{
			try
			{
				await _eventAreaService.Update(EventAreaParser.ToEventAreaDto(entity));
			}
			catch (EventSeatException exception)
			{
				var fault = new ServiceValidationFaultDetails { Message = "Update event seat error" };
				throw new FaultException<ServiceValidationFaultDetails>(fault, exception.Message);
			}
			catch (EventAreaException exception)
			{
				var fault = new ServiceValidationFaultDetails { Message = "Update event area error" };
				throw new FaultException<ServiceValidationFaultDetails>(fault, exception.Message);
			}
		}
    }
}
