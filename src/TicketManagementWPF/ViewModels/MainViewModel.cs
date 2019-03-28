using System;
using System.Threading.Tasks;
using TicketManagementWPF.Helpers.WindowManagement;
using TicketManagementWPF.Infrastructure;
using TicketManagementWPF.Infrastructure.Extensions.Localization;

namespace TicketManagementWPF.ViewModels
{
	public class MainViewModel : ViewModelAbstract, IDisplayWindow
	{
        public const string ChangeDisplayContentOperationKey = "ChangeMainWindowContent";
		public const string ChangeDisplayTitleOperationKey = "ChangeMainWindowTitle";

		#region Binding properties

		private object _displayView;
		public object DisplayView
		{
			get
			{
				return _displayView;
			}
			set
			{
				DisposePreviousViewModel(_displayView);
				_displayView = value;
				OnPropertyChanged();
			}
		}

		private string _title;
		public string Title
		{
			get
			{
				return _title;
			}
			set
			{
				_title = value;
				OnPropertyChanged();
			}
		}

		#endregion

		private readonly IMediator _mediator;
		private readonly IWindowHelper _windowHelper;

		public MainViewModel(IWindowHelper windowHelper, IMediator mediator)
		{
			_mediator = mediator;
			_windowHelper = windowHelper;
		}

        private void MainWindowContentChange(object obj)
        {
			DisplayView = obj;
        }       

		public override async Task Initialize()
		{
			_mediator.Subscribe(ChangeDisplayContentOperationKey, MainWindowContentChange);
			_mediator.Subscribe(ChangeDisplayTitleOperationKey, SetTitle);
			TranslateSource.Instance.PropertyChanged += CultureChanged;

			//set up start page
			Title = TranslateSource.Instance[nameof(l10n.Login.View.Title), l10n.Login.View.ResourceManager.BaseName];
			var viewModel = await _windowHelper.GetViewModel<LoginViewModel>();
			DisplayView = viewModel;
		}

		private void CultureChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			Title = TranslateSource.Instance[nameof(l10n.Login.View.Title), l10n.Login.View.ResourceManager.BaseName];
		}

		private void DisposePreviousViewModel(object viewModel)
		{
			if (viewModel is null)
				return;

			if (!(viewModel is ViewModelAbstract vm))
				return;

            vm.Dispose();
		}

        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                _mediator.Unsubscribe(ChangeDisplayContentOperationKey, MainWindowContentChange);
				TranslateSource.Instance.PropertyChanged -= CultureChanged;
			}

            disposed = true;
        }

		private void SetTitle(object obj) => Title = obj is string ? Convert.ToString(obj) : Title;
    }
}
