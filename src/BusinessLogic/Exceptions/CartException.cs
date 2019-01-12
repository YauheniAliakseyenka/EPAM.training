using System;

namespace BusinessLogic.Exceptions
{
	internal class CartException : Exception
	{
		public CartException()
		{
		}

		public CartException(string message)
			: base(message)
		{
		}

		public CartException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
