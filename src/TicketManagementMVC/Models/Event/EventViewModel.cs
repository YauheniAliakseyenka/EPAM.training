using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TicketManagementMVC.Infrastructure.Attributes;

namespace TicketManagementMVC.Models.Event
{
	public class EventViewModel
	{
		public int Id { get; set; }
		
		[Display(Name = "Title", ResourceType = typeof(I18N.Resource))]
		[Required(ErrorMessageResourceType = typeof(I18N.ResourceErrors), ErrorMessageResourceName = "PropertyRequired")]
		public string Title { get; set; }

		[Required(ErrorMessageResourceName = "PropertyRequired", ErrorMessageResourceType = typeof(I18N.ResourceErrors))]
		[Display(Name = "DescriptionLabel", ResourceType = typeof(I18N.Resource))]
		public string Description { get; set; }

		[Required(ErrorMessageResourceName = "PropertyRequired", ErrorMessageResourceType = typeof(I18N.ResourceErrors))]
		[Display(Name = "Layout", ResourceType = typeof(I18N.Resource))]
		public int LayoutId { get; set; }

		[DataType(DataType.Time)]
		[Required(ErrorMessageResourceName = "PropertyRequired", ErrorMessageResourceType = typeof(I18N.ResourceErrors))]
		[Display(Name = "Time", ResourceType = typeof(I18N.Resource))]		
		public DateTime Time { get; set; }

		[DataType(DataType.Date)]
		[Required(ErrorMessageResourceName = "PropertyRequired", ErrorMessageResourceType = typeof(I18N.ResourceErrors))]
		[Display(Name = "Date", ResourceType = typeof(I18N.Resource))]		
		public DateTime Date { get; set; }

		public string ImageURL { get; set; }
	}
}