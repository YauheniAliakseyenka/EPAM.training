using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    [Table(name: "OrderedSeat")]
    public class OrderedSeat
    {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		[Column(name: "SeatId")]
		public int SeatId { get; set; }
		[Column(name: "CartId")]
        public int CartId { get; set; }
	}
}
