using System.Transactions;

namespace BusinessLogic.Services
{
	internal class CustomTransactionScope
	{
		public static TransactionScope GetTransactionScope()
		{
			var transactionOptions = new TransactionOptions
			{
				IsolationLevel = IsolationLevel.ReadUncommitted,
				Timeout = TransactionManager.MaximumTimeout,
			};
			return new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled);
		}
	}
}
