using System.Threading.Tasks;
using TicketManagementWPF.Infrastructure;

namespace TicketManagementWPF.Helpers.WindowManagement
{
	public interface IWindowHelper
	{
		Task<object> GetViewModel<T>() where T : ViewModelAbstract;
		Task<object> Show<T>() where T : ViewModelAbstract;
		Task<object> ShowDialog<T>() where T : ViewModelAbstract;
        void ShowDialog(ViewModelAbstract viewModel);
	}
}
