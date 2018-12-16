using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Exceptions
{
	internal class SeatException : AreaException
	{
		public SeatException()
		{
		}

		public SeatException(string message)
			: base(message)
		{
		}

		public SeatException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
