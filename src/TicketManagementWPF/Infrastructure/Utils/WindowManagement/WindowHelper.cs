using System.Threading.Tasks;
using System.Windows;
using TicketManagementWPF.Autofac;
using TicketManagementWPF.Infrastructure;
using TicketManagementWPF.Views.Windows;

namespace TicketManagementWPF.Helpers.WindowManagement
{
    internal class WindowHelper : IWindowHelper
	{
		public async Task<object> GetViewModel<T>() where T : ViewModelAbstract
		{
			var viewModel = AutofacContainer.Resolve<T>() as ViewModelAbstract;
			await viewModel?.Initialize();

			return viewModel;
		}

		public async Task<object> Show<T>() where T : ViewModelAbstract
		{
			var viewModel = await GetViewModel<T>();
			var window = CreateWindow(viewModel);
			window.Show();

			return viewModel;
		}

		public async Task<object> ShowDialog<T>() where T : ViewModelAbstract
		{
			var viewModel = await GetViewModel<T>();
			var window = CreateWindow(viewModel);
			window.ShowDialog();

			return viewModel;
		}

		public  void ShowDialog(ViewModelAbstract viewModel)
		{
			var window = CreateWindow(viewModel);
			window.ShowDialog();
		}

		private Window CreateWindow(object context)
		{
			var window = new AnyWindow();
			window.DataContext = context;

			return window;
		}
	}
} 
