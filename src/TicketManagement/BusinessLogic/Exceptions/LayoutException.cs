using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Exceptions
{
	internal class LayoutException: VenueException
	{
		public LayoutException()
		{
		}

		public LayoutException(string message)
			: base(message)
		{
		}

		public LayoutException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
