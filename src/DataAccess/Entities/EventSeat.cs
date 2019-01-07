using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
	[Table(name: "EventSeat")]
	public class EventSeat
	{
		[Column(name: "Id")]
		public int Id { get; set; }
		[Column(name: "EventAreaId")]
		public int EventAreaId { get; set; }
		[Column(name: "Row")]
		public int Row { get; set; }
		[Column(name: "Number")]
		public int Number { get; set; }
		[Column(name: "State")]
		public int State { get; set; }
	}
}
