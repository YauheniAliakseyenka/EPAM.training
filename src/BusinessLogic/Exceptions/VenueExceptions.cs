using System;

namespace BusinessLogic.Exceptions.VenueExceptions
{
	internal class AreaException : LayoutException
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

	internal class VenueException : Exception
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

	internal class LayoutException : VenueException
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
