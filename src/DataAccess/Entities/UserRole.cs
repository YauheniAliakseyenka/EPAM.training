using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
	[Table(name:"UserRole")]
	public class UserRole 
	{
		[Column(name: "UserId")]
		public string UserId { get; set; }
		[Column(name: "RoleId")]
		public int RoleId { get; set; }
	}
}
