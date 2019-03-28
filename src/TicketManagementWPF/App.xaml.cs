using System.Windows;
using System.Windows.Threading;
using TicketManagementWPF.Autofac;
using TicketManagementWPF.Helpers.WindowManagement;
using TicketManagementWPF.ViewModels;

namespace TicketManagementWPF
{
	public partial class App : Application
	{
		public App()
		{
			this.Dispatcher.UnhandledException += GlobalExceptionHandler;
		}

		private void GlobalExceptionHandler(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			string errorMessage = string.Format("An unhandled exception occurred: {0}", e.Exception.Message);
			MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			e.Handled = true;
		}

		private void App_Startup(object sender, StartupEventArgs e)
		{
			AutofacContainer.Config();
		}

		protected override async void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			var windowHelper = AutofacContainer.Resolve<IWindowHelper>();

			//open main window
			await windowHelper.Show<MainViewModel>();
		}
	}
}
