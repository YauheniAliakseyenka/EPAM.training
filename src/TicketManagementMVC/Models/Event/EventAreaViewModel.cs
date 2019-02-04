using BusinessLogic.DTO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TicketManagementMVC.Infrastructure.Attributes;

namespace TicketManagementMVC.Models.Event
{
	public class EventAreaViewModel
	{
		public int Id { get; set; }
		public int EventId { get; set; }

		[Required(ErrorMessageResourceName = "PropertyRequired", ErrorMessageResourceType = typeof(ProjectResources.ResourceErrors))]
		[Display(Name = "Description", ResourceType = typeof(ProjectResources.EventResource))]
		public string Description { get; set; }

		[Required(ErrorMessageResourceName = "PropertyRequired", ErrorMessageResourceType = typeof(ProjectResources.ResourceErrors))]
		[ValidInteger]
		[Display(Name = "CoordX", ResourceType = typeof(ProjectResources.EventResource))]
		public int CoordX { get; set; }

		[Required(ErrorMessageResourceName = "PropertyRequired", ErrorMessageResourceType = typeof(ProjectResources.ResourceErrors))]
		[ValidInteger]
		[Display(Name = "CoordY", ResourceType = typeof(ProjectResources.EventResource))]
		public int CoordY { get; set; }

		[Required(ErrorMessageResourceName = "PropertyRequired", ErrorMessageResourceType = typeof(ProjectResources.ResourceErrors))]
		[Display(Name = "PriceCurrency", ResourceType = typeof(ProjectResources.EventResource))]
		[DataType(DataType.Currency)]
		public decimal Price { get; set; }

		[Required(ErrorMessageResourceName = "SeatValdiatation", ErrorMessageResourceType = typeof(ProjectResources.ResourceErrors))]
		public List<EventSeatDto> SeatList { get; set; }
	}
}