using System;
using System.Threading.Tasks;
using System.Windows.Input;
using TicketManagementWPF.Helpers.WindowManagement;
using TicketManagementWPF.Infrastructure;
using TicketManagementWPF.Infrastructure.Commands;
using TicketManagementWPF.Infrastructure.Extensions.Localization;
using TicketManagementWPF.Infrastructure.Utils.UserManagement;

namespace TicketManagementWPF.ViewModels
{
    public class MainMenuViewModel : ViewModelAbstract
	{
        public const string ChangeDisplayContentOperationKey = "ChangeMainMenuContent";

        #region Properties

        private object _currentContent;
		public object CurrentContent
		{
			get
			{
				return _currentContent;
			}
			set
			{
                _currentContent = value;
				OnPropertyChanged();
			}
		}

        #endregion

        #region Commands

        public ICommand VenueManagementCommand { get; set; }
		public ICommand UserManagementCommand { get; set; }
        public ICommand ProfileCommand { get; set; }

		#endregion

		private string TitleResourceKey;
		private string TitleResourcePath;

		private readonly IWindowHelper _windowHelper;
		private readonly IMediator _mediator;
		private readonly IUserProfile _userManager;

		public MainMenuViewModel(
			IWindowHelper windowHelper, 
			IMediator mediator,
			IUserProfile userManager)
		{
            _windowHelper = windowHelper;
			_mediator = mediator;
			_userManager = userManager;
		}

        private async Task OnVenueManagement(object obj)
        {
			TitleResourceKey = nameof(l10n.VenueManagement.View.Title);
			TitleResourcePath = l10n.VenueManagement.View.ResourceManager.BaseName;

			_mediator.Raise(MainViewModel.ChangeDisplayTitleOperationKey,
				TranslateSource.Instance[TitleResourceKey, TitleResourcePath]);

			var viewModel = await _windowHelper.GetViewModel<VenueManagementViewModel>();
            CurrentContent = viewModel;
		}

		private async Task OnUserManagement(object obj)
		{
			TitleResourceKey = nameof(l10n.UserManagement.View.Title);
			TitleResourcePath = l10n.UserManagement.View.ResourceManager.BaseName;

			_mediator.Raise(MainViewModel.ChangeDisplayTitleOperationKey,
				TranslateSource.Instance[TitleResourceKey, TitleResourcePath]);

			var viewModel = await _windowHelper.GetViewModel<UserManagementViewModel>();
            CurrentContent = viewModel;
		}

        private async Task OnProfile(object obj)
        {
            var viewModel = await _windowHelper.GetViewModel<ProfileViewModel>() as ProfileViewModel;
            var response = await _userManager.Get();

			if (!response.IsSuccess)
				throw new Exception(l10n.Shared.Errors.InternalAppError);
		
			var user = response.Object as Models.User;

			TitleResourceKey = nameof(l10n.Shared.SharedResources.ProfileTitle);
			TitleResourcePath = l10n.Shared.SharedResources.ResourceManager.BaseName;

			_mediator.Raise(MainViewModel.ChangeDisplayTitleOperationKey, TranslateSource.Instance[TitleResourceKey, TitleResourcePath]);
			viewModel.User = user;
            CurrentContent = viewModel;
        }

		private void ChangeDisaplyContent(object obj)
		{
            CurrentContent = obj;
		}

		public override async Task Initialize()
		{
			_mediator.Subscribe(ChangeDisplayContentOperationKey, ChangeDisaplyContent);
			TranslateSource.Instance.PropertyChanged += CultureChanged;
			CurrentContent = await _windowHelper.GetViewModel<VenueManagementViewModel>();
			VenueManagementCommand = new RelayCommandAsync(OnVenueManagement);
			UserManagementCommand = new RelayCommandAsync(OnUserManagement);
			ProfileCommand = new RelayCommandAsync(OnProfile);
		}

		private void CultureChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			var titleTemp = TranslateSource.Instance[TitleResourceKey, TitleResourcePath];
			_mediator.Raise(MainViewModel.ChangeDisplayTitleOperationKey, titleTemp);
		}

		protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                _mediator.Unsubscribe(ChangeDisplayContentOperationKey, ChangeDisaplyContent);
            }

            disposed = true;
        }
    }
}
