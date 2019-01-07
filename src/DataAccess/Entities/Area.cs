using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
	[Table(name:"Area")]
	public class Area
    {
		[Column (name:"Id")]
	    public int Id { get; set; }
	    [Column(name: "LayoutId")]
		public int LayoutId { get; set; }
	    [Column(name: "Description")]
		public string Description { get; set; }
	    [Column(name: "CoordX")]
		public int CoordX { get; set; }
	    [Column(name: "CoordY")]
		public int CoordY { get; set; }
	}
}
