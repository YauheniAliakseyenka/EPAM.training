using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
	[Table(name: "Event")]
	public class Event
	{
		[Key]
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
		public int CreatedBy { get; set; }
	}
}
