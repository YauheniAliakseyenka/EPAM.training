using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    [Table(name: "Cart")]
    public class Cart
    {
		[Key]
		[Column(name: "Id")]
        public int Id { get; set; }
        [Column(name: "UserId")]
        public int UserId { get; set; }
    }
}
