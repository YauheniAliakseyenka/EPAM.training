using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
	[Table(name: "Event")]
	public class Event
	{
		[Column(name: "Id")]
		public int Id { get; set; }
		[Column(name: "Title")]
		public string Title { get; set; }
		[Column(name: "Description")]
		public string Description { get; set; }
		[Column(name: "ImageURL")]
		public string ImageURL { get; set; }
		[Column(name: "LayoutId")]
		public int LayoutId { get; set; }
		[Column(name: "Date")]
		public DateTime Date { get; set; }
		[Column(name: "CreatedBy")]
		public string CreatedBy { get; set; }
	}
}
