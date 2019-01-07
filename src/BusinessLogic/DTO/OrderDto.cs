using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTO
{
	public class OrderDto
	{
		public int Id { get; set; }
		public string UserId { get; set; }
		public DateTime Date { get; set; }
	}
}
