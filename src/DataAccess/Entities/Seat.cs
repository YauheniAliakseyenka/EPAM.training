using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
	[Table(name:"Seat")]
    public class Seat
	{
		[Column(name: "Id")]
		public int Id { get; set; }
		[Column(name: "AreaId")]
		public int AreaId { get; set; }
		[Column(name: "Row")]
		public int Row { get; set; }
		[Column(name: "Number")]
		public int Number { get; set; }
    }
}
