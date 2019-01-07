using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
	[Table(name:"EventArea")]
    public class EventArea
	{
		[Column(name: "Id")]
		public int Id { get; set; }
		[Column(name: "EventId")]
		public int EventId { get; set; }
		[Column(name: "Description")]
		public string Description { get; set; }
		[Column(name: "CoordX")]
		public int CoordX { get; set; }
		[Column(name: "CoordY")]
		public int CoordY { get; set; }
		[Column(name: "Price")]
		public decimal Price { get; set; }
		[Column(name: "AreaDefaultId")]
		public int AreaDefaultId { get; set; }
	}
}
