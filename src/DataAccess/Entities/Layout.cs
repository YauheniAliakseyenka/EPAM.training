using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
	[Table(name:"Layout")]
    public class Layout
	{
		[Key]
		[Column(name: "Id")]
		public int Id { get; set; }
		[Column(name: "VenueId")]
		public int VenueId { get; set; }
		[Column(name: "Description")]
		public string Description { get; set; }
	}
}
