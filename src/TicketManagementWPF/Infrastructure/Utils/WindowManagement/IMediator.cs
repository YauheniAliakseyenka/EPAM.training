using System;

namespace TicketManagementWPF.Helpers.WindowManagement
{
	public interface IMediator
	{
		void Subscribe(string token, Action<object> callback);
		void Unsubscribe(string token, Action<object> callback);
		void ClearCallBacksByKey(string token);
		void Raise(string token, object args = null);
	}
}
