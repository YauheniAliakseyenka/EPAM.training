using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    [Table(name: "Order")]
    public class Order
    {
		[Key]
		[Column(name: "Id")]
        public int Id { get; set; }
        [Column(name: "UserId")]
        public int UserId { get; set; }
        [Column(name: "Date")]
        public DateTimeOffset Date { get; set; }
    }
}
