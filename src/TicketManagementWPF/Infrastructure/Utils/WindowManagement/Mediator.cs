using System;
using System.Collections.Generic;
using System.Linq;

namespace TicketManagementWPF.Helpers.WindowManagement
{
	public class Mediator : IMediator
	{
		private IDictionary<string, List<Action<object>>> pl_dict =
		   new Dictionary<string, List<Action<object>>>();

		public void Subscribe(string token, Action<object> callback)
		{
			if (!pl_dict.ContainsKey(token))
			{
				var list = new List<Action<object>>();
				list.Add(callback);
				pl_dict.Add(token, list);
			}
			else
			{
				bool found = false;
				foreach (var item in pl_dict[token])
					if (ReferenceEquals(item.Target, callback.Target))
						found = true;
				if (!found)
					pl_dict[token].Add(callback);
			}
		}

		public void Unsubscribe(string token, Action<object> callback)
		{
			if (pl_dict.ContainsKey(token))
			{
				pl_dict[token].Remove(callback);

				if (!pl_dict[token].Any())
					pl_dict.Remove(token);
			}
		}

		public void ClearCallBacksByKey(string token)
		{
			if (pl_dict.ContainsKey(token))
				pl_dict.Remove(token);
		}

		public void Raise(string key, object args = null)
		{
			if (pl_dict.ContainsKey(key))
				foreach (var callback in pl_dict[key])
					callback(args);
		}
	}
}
