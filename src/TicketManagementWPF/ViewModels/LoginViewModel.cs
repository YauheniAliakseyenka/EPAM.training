using System.Collections.Generic;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Input;
using TicketManagementWPF.Helpers.WindowManagement;
using TicketManagementWPF.Infrastructure;
using TicketManagementWPF.Infrastructure.Commands;
using TicketManagementWPF.Infrastructure.Utils.Identity;
using TicketManagementWPF.Infrastructure.Utils.UserManagement;
using TicketManagementWPF.Models;
using TicketManagementWPF.Infrastructure.Extensions;

namespace TicketManagementWPF.ViewModels
{
	internal class LoginViewModel : ViewModelAbstract
	{
		#region Binding properties

		private string _userName;
		public string UserName
		{
			get { return _userName; }
			set
			{
				_userName = value;
				OnPropertyChanged();
			}
		}

		private SecureString _password;
		public SecureString Password
		{
			get { return _password; }
			set
			{
				_password = value;
				OnPropertyChanged();
			}
		}

        private List<string> _errors;
        public List<string> Errors
        {
            get { return _errors; }
            set
            {
                _errors = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand SingInCommand { get; private set; }

		#endregion

		private readonly IWindowHelper _windowHelper;
		private readonly IMediator _mediator;
        private readonly IAuth _userManager;
        private readonly IUserIdentity _identity;

        public LoginViewModel(
            IWindowHelper windowHelper, 
            IMediator mediator,
            IAuth userManager,
            IUserIdentity identity)
		{
            _windowHelper = windowHelper;
			_mediator = mediator;
            _userManager = userManager;
            _identity = identity;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj">Password</param>
		/// <returns></returns>
		private async Task SignIn(object obj)
		{
			if (string.IsNullOrEmpty(UserName) || Password is null || Password.Length == 0)
			{
				DisplayError(l10n.Login.Errors.LoginError);
				return;
			}

			var result = await CheckCredentials();

			if (result && _identity.IsAdmin())
			{
				var viewModel = await _windowHelper.GetViewModel<MainMenuViewModel>();
				_mediator.Raise(MainViewModel.ChangeDisplayContentOperationKey, viewModel);
			}
			else if (result && !_identity.IsAdmin())
				DisplayError(l10n.Login.Errors.AccessDedined);
			else
				DisplayError(l10n.Login.Errors.LoginError);
		}

		private async Task<bool> CheckCredentials()
		{
            var credentials = new AuthModel
            {
                Password = Password.GetStringValue(),
                Username = UserName
            };
            var result = await _userManager.SignIn(credentials);

			if (!result.IsSuccess)
				return false;
            
			var tokens = result.Object as TokenModel;
            _identity.SaveTokens(tokens);

			return true;
		}

		public override Task Initialize()
		{
			SingInCommand = new RelayCommandAsync(SignIn);

			return Task.FromResult(0);
		}

		private void DisplayError(string error)
		{
			if (Errors is null)
				Errors = new List<string>();
			else
				Errors.Clear();

			Errors.Add(error);
			OnPropertyChanged(nameof(Errors));
		}
	}
}
