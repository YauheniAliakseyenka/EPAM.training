using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    [Table(name: "RefreshToken")]
    public class RefreshToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(name: "UserId")]
        public int UserId { get; set; }
        [Column(name: "Token")]
        public string Token { get; set; }
    }
}
