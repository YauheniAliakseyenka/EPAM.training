using BusinessLogic.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TicketManagementMVC.Infrastructure;
using TicketManagementMVC.Infrastructure.Attributes;

namespace TicketManagementMVC.Models.Event
{
	public class EventAreaViewModel
	{
		public int Id { get; set; }
		public int EventId { get; set; }

		[Required(ErrorMessageResourceName = "PropertyRequired", ErrorMessageResourceType = typeof(I18N.ResourceErrors))]
		[Display(Name = "DescriptionLabel", ResourceType = typeof(I18N.Resource))]
		public string Description { get; set; }

		[Required(ErrorMessageResourceName = "PropertyRequired", ErrorMessageResourceType = typeof(I18N.ResourceErrors))]
		[ValidInteger]
		[Display(Name = "CoordX", ResourceType = typeof(I18N.Resource))]
		public int CoordX { get; set; }

		[Required(ErrorMessageResourceName = "PropertyRequired", ErrorMessageResourceType = typeof(I18N.ResourceErrors))]
		[ValidInteger]
		[Display(Name = "CoordY", ResourceType = typeof(I18N.Resource))]
		public int CoordY { get; set; }

		[Required(ErrorMessageResourceName = "PropertyRequired", ErrorMessageResourceType = typeof(I18N.ResourceErrors))]
		[Display(Name = "Price", ResourceType = typeof(I18N.Resource))]
		[DataType(DataType.Currency)]
		public decimal Price { get; set; }

		[Required(ErrorMessageResourceName = "SeatValdiatation", ErrorMessageResourceType = typeof(I18N.ResourceErrors))]
		public List<EventSeatDto> SeatList { get; set; }
	}
}