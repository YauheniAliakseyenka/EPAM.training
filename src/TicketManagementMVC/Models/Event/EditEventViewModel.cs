using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TicketManagementMVC.EventService;

namespace TicketManagementMVC.Models.Event
{
    public class EditEventViewModel
    {
		[Required]
        public EventViewModel Event { get; set; }
        public List<EventArea> EventAreas { get; set; }
    }
}