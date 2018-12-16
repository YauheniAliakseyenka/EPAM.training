using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Exceptions
{
	internal class EventException :Exception
	{
		public EventException()
		{
		}

		public EventException(string message)
			: base(message)
		{
		}

		public EventException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
