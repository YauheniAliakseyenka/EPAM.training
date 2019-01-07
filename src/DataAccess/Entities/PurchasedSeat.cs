using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    [Table(name: "PurchasedSeat")]
    public class PurchasedSeat
    {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		[Column(name: "SeatId")]
        public int SeatId { get; set; }
		[Column(name: "OrderId")]
		public int OrderId { get; set; }
		[Column(name: "Price")]
        public decimal Price { get; set; }
    }
}
