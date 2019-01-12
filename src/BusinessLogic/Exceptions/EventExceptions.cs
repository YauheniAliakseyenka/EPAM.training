using System;

namespace BusinessLogic.Exceptions.EventExceptions
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

	internal class EventException : Exception
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
