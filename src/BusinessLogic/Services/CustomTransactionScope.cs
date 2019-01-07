using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BusinessLogic.Services
{
	internal class CustomTransactionScope
	{
		public static TransactionScope GetTransactionScope()
		{
			var transactionOptions = new TransactionOptions
			{
				IsolationLevel = IsolationLevel.ReadCommitted,
				Timeout = TransactionManager.MaximumTimeout
			};
			return new TransactionScope(TransactionScopeOption.Required, transactionOptions);
		}
	}
}
