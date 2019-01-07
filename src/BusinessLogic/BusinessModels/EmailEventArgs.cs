using BusinessLogic.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.BusinessModels
{
	public class EmailEventArgs : EventArgs
	{
		public UserDto User { get; set; }
		public OrderModel OrderModel { get; set; }
	}
}
