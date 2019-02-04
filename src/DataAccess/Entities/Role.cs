using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
	[Table(name:"Role")]
	public class Role
	{
		[Key]
		[Column(name: "Id")]
		public int Id { get; set; }
		[Column(name: "Name")]
		public string Name { get; set; }
	}
}
