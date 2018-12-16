using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Exceptions
{
	internal class EventSeatException : EventAreaException
	{
		public EventSeatException()
		{
		}

		public EventSeatException(string message)
			: base(message)
		{
		}

		public EventSeatException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
