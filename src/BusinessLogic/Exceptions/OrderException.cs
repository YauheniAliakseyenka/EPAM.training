using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Exceptions
{
	internal class OrderException :Exception
	{
		public OrderException()
		{
		}

		public OrderException(string message)
			: base(message)
		{
		}

		public OrderException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
