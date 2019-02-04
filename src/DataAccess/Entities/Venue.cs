using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
	[Table(name:"Venue")]
    public class Venue
	{
		[Key]
		[Column(name: "Id")]
		public int Id { get; set; }
		[Column(name: "Name")]
		public string Name { get; set; }
		[Column(name: "Address")]
		public string Address { get; set; }
		[Column(name: "Phone")]
		public string Phone { get; set; }
		[Column(name: "Description")]
		public string Description { get; set; }
		[Column(name: "Timezone")]
		public string Timezone { get; set; }
	}
}
