using BusinessLogic.DTO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TicketManagementMVC.Models.Event
{
    public class EditEventViewModel
    {
		[Required]
        public EventViewModel Event { get; set; }
        public List<EventAreaDto> EventAreas { get; set; }
    }
}