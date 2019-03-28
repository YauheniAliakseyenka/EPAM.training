using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TicketManagementWPF.Infrastructure.Commands
{
    internal class RelayCommandAsync : IAsyncCommand
	{
		private readonly Func<object, Task> _executeAsync;
		private readonly Func<object, bool> _canExecute;

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public RelayCommandAsync(Func<object, Task> execute, Func<object, bool> canExecute = null)
		{
			this._executeAsync = execute;
			this._canExecute = canExecute;
		}

		public bool CanExecute(object parameter)
		{
			return this._canExecute == null || this._canExecute(parameter);
		}

		public async void Execute(object parameter)
		{
			await this._executeAsync(parameter);
		}

		public async Task ExecuteAsync(object parameter)
		{
			await this._executeAsync(parameter);
		}
	}
}
