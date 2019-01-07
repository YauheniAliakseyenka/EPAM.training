using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
	[Table(name:"Role")]
	public class Role
	{
		[Column(name: "Id")]
		public int Id { get; set; }
		[Column(name: "Name")]
		public string Name { get; set; }
	}
}
