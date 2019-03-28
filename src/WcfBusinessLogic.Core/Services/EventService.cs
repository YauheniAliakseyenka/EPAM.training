using BusinessLogic.Exceptions.EventExceptions;
using BusinessLogic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Security.Permissions;
using WcfBusinessLogic.Core.Contracts.Services;
using WcfBusinessLogic.Core.Helpers.Parsers;
using WcfBusinessLogic.Core.Contracts.Data;
using WcfBusinessLogic.Core.Contracts.Exceptions;

namespace WcfBusinessLogic.Core.Services
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
	internal class WcfEventService : IWcfEventService
    {
        private IEventService _eventService;

        public WcfEventService(IEventService eventService)
        {
            _eventService = eventService;
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Manager")]
        public async Task<int> Create(Event entity)
        {
			try
			{
				var add = EventParser.ToEventDto(entity);
				await _eventService.Create(add);
                return add.Id;
            }
			catch(EventException exception)
			{
				var fault = new ServiceValidationFaultDetails { Message = "Create event error" };
				throw new FaultException<ServiceValidationFaultDetails>(fault, exception.Message);
			}
        }
		
        [PrincipalPermission(SecurityAction.Demand, Role = "Manager")]
        public async Task Update(Event entity)
		{
            try
            {
                await _eventService.Update(EventParser.ToEventDto(entity));
            }
            catch (EventException exception)
            {
                var fault = new ServiceValidationFaultDetails { Message = "Update event error" };
                throw new FaultException<ServiceValidationFaultDetails>(fault, exception.Message);
            }
		}
		
        [PrincipalPermission(SecurityAction.Demand, Role = "Manager")]
        public async Task Delete(int id)
        {
            try
            {
                await _eventService.Delete(id);
            }
            catch (EventException exception)
            {
                var fault = new ServiceValidationFaultDetails { Message = "Delete event error" };
                throw new FaultException<ServiceValidationFaultDetails>(fault, exception.Message);
            }
        }
		
        [PrincipalPermission(SecurityAction.Demand, Role = "Manager")]
        public async Task<Event> Get(int id)
        {
            try
            {
                var result = await _eventService.Get(id);

                return EventParser.ToEventContract(result);
            }
            catch (EventException exception)
            {
                var fault = new ServiceValidationFaultDetails { Message = "Get event error" };
                throw new FaultException<ServiceValidationFaultDetails>(fault, exception.Message);
            }
        }
		
        [PrincipalPermission(SecurityAction.Demand, Role = "Manager")]
        public async Task<IEnumerable<Event>> ToList()
        {
            try
            {
                var result = await _eventService.GetList();

                return result.Select(x => EventParser.ToEventContract(x));
            }
            catch (EventException exception)
            {
                var fault = new ServiceValidationFaultDetails { Message = "Get list of events error" };
                throw new FaultException<ServiceValidationFaultDetails>(fault, exception.Message);
            }
        }
		
        [PrincipalPermission(SecurityAction.Demand, Role = "Manager")]
        public async Task<IEnumerable<EventBusinessModel>> GetPublishedEvents(Contracts.Data.FilterEventOptions filter, string filterText = null, string dateCulture = null)
        {
            try
            {
                BusinessLogic.Services.FilterEventOptions bllFilter;
                if (Enum.IsDefined(typeof(BusinessLogic.Services.FilterEventOptions), filter.ToString()))
                    bllFilter = (BusinessLogic.Services.FilterEventOptions)filter;
                else
                {
                    var fault = new ServiceValidationFaultDetails { Message = "Filter parsing error" };
                    var errorMessage = "Picked filter option is not defined";
                    throw new FaultException<ServiceValidationFaultDetails>(fault, errorMessage);
                }

                var events = await _eventService.GetPublishedEvents(bllFilter, filterText, dateCulture);

                return events.Select(x => EventModelBllToEventModelContract(x));
            }
            catch (EventException exception)
            {
                var fault = new ServiceValidationFaultDetails { Message = "Get published list of events error" };
                throw new FaultException<ServiceValidationFaultDetails>(fault, exception.Message);
            }
        }

        private EventBusinessModel EventModelBllToEventModelContract(BusinessLogic.BusinessModels.EventModel from)
        {
			return new EventBusinessModel
			{
				Areas = from.Areas?.Select(x => EventAreaParser.ToEventAreaContract(x)).ToList(),
				Event = from.Event is null ? null : EventParser.ToEventContract(from.Event),
				IsPublished = from.IsPublished,
				LayoutName = from.LayoutName ?? null,
				Venue = from.Venue is null ? null : VenueParser.ToVenueContract(from.Venue)
			};
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Manager")]
        public async Task<EventBusinessModel> GetEventInformation(int id)
        {
            try
            {
                var result = await _eventService.GetEventInformation(id);

                return EventModelBllToEventModelContract(result);
            }
            catch (EventException exception)
            {
                var fault = new ServiceValidationFaultDetails { Message = "Get event information error" };
                throw new FaultException<ServiceValidationFaultDetails>(fault, exception.Message);
            }
        }
		
        [PrincipalPermission(SecurityAction.Demand, Role = "Manager")]
        public async Task<IEnumerable<Event>> GetEventManagerEvents(int venueId, int userId)
        {
            try
            {
                var result = await _eventService.GetEventManagerEvents(venueId, userId);

                return result.Select(x => EventParser.ToEventContract(x));
            }
            catch (EventException exception)
            {
                var fault = new ServiceValidationFaultDetails { Message = "Get event manager events error" };
                throw new FaultException<ServiceValidationFaultDetails>(fault, exception.Message);
            }
        }

		[PrincipalPermission(SecurityAction.Demand, Role = "Manager")]
		public bool HasLockedSeats(int id)
		{
			return _eventService.HasLockedSeats(id);
		}
	}
}
