using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Exceptions
{
	internal class AreaException: LayoutException
	{
		public AreaException()
		{
		}

		public AreaException(string message)
			: base(message)
		{
		}

		public AreaException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
