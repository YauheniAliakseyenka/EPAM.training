using System.Threading.Tasks;
using System.Windows.Input;

namespace TicketManagementWPF.Infrastructure.Commands
{
    public interface IAsyncCommand : ICommand
    {
		Task ExecuteAsync(object parameter);
	}
}
