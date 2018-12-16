using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Exceptions
{
	internal class EventAreaException : EventException
	{
		public EventAreaException()
		{
		}

		public EventAreaException(string message)
			: base(message)
		{
		}

		public EventAreaException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
