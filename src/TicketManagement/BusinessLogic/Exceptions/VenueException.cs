using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Exceptions
{
	internal class VenueException :Exception
	{
		public VenueException()
		{
		}

		public VenueException(string message)
			: base(message)
		{
		}

		public VenueException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
