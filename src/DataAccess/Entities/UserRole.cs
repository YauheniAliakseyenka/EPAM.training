using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
	[Table(name:"UserRole")]
	public class UserRole 
	{
		[Key]
		[Column(name: "UserId", Order = 1)]
		public int UserId { get; set; }
		[Key]
		[Column(name: "RoleId", Order = 2)]
		public int RoleId { get; set; }
	}
}
