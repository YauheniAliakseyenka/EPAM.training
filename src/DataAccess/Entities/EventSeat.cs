using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
	[Table(name: "EventSeat")]
	public class EventSeat
	{
		[Key]
		[Column(name: "Id")]
		public int Id { get; set; }
		[Column(name: "EventAreaId")]
		public int EventAreaId { get; set; }
		[Column(name: "Row")]
		public int Row { get; set; }
		[Column(name: "Number")]
		public int Number { get; set; }
		[Column(name: "State")]
		public byte State { get; set; }
    }
}
