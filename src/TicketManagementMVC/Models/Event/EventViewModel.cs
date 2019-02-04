using System;
using System.ComponentModel.DataAnnotations;

namespace TicketManagementMVC.Models.Event
{
	public class EventViewModel
	{
		public int Id { get; set; }
		
		[Display(Name = "Title", ResourceType = typeof(ProjectResources.EventResource))]
		[Required(ErrorMessageResourceType = typeof(ProjectResources.ResourceErrors), ErrorMessageResourceName = "PropertyRequired")]
		public string Title { get; set; }

		[Required(ErrorMessageResourceName = "PropertyRequired", ErrorMessageResourceType = typeof(ProjectResources.ResourceErrors))]
		[Display(Name = "Description", ResourceType = typeof(ProjectResources.EventResource))]
		public string Description { get; set; }

		[Required(ErrorMessageResourceName = "PropertyRequired", ErrorMessageResourceType = typeof(ProjectResources.ResourceErrors))]
		[Display(Name = "Layout", ResourceType = typeof(ProjectResources.EventResource))]
		public int LayoutId { get; set; }

		[DataType(DataType.Time)]
		[Required(ErrorMessageResourceName = "PropertyRequired", ErrorMessageResourceType = typeof(ProjectResources.ResourceErrors))]
		[Display(Name = "Time", ResourceType = typeof(ProjectResources.EventResource))]		
		public DateTime Time { get; set; }

		[DataType(DataType.Date)]
		[Required(ErrorMessageResourceName = "PropertyRequired", ErrorMessageResourceType = typeof(ProjectResources.ResourceErrors))]
		[Display(Name = "Date", ResourceType = typeof(ProjectResources.EventResource))]		
		public DateTime Date { get; set; }

		public string ImageURL { get; set; }
		public string VenueTimezone { get; set; }
	}
}