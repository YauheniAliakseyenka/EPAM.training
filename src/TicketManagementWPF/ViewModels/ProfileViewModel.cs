using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Input;
using TicketManagementWPF.Helpers.WindowManagement;
using TicketManagementWPF.Infrastructure;
using TicketManagementWPF.Infrastructure.Commands;
using TicketManagementWPF.Infrastructure.Utils.UserManagement;
using TicketManagementWPF.Infrastructure.Utils.Validation;
using TicketManagementWPF.Infrastructure.Validation.Utils;
using TicketManagementWPF.Infrastructure.Extensions;

namespace TicketManagementWPF.ViewModels
{
    internal class ProfileViewModel : ViewModelAbstract
    {
        #region Binding properties

        private Models.User _user;
        public Models.User User
        {
            get { return _user; }
            set
            {
                _user = value;
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

        private SecureString _password;
        [DataType(DataType.Password)]
        [CustomRequired("PasswordLabel", typeof(l10n.UserView.View))]
        [MinLength(10, 
            ErrorMessageResourceName = "PasswordLength", 
            ErrorMessageResourceType = typeof(l10n.UserView.Errors))]
        public SecureString Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        private SecureString _confirmedPassword;
        [DataType(DataType.Password)]
        [PasswordCompare(nameof(Password), 
            ErrorMessageResourceName = "ConfirmPassword", 
            ErrorMessageResourceType = typeof(l10n.UserView.Errors))]
        public SecureString ConfirmedPassword
        {
            get { return _confirmedPassword; }
            set
            {
                _confirmedPassword = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand SaveCommand { get; private set; }

        #endregion

        private readonly IUserProfile _userManager;

        public ProfileViewModel(IUserProfile userManager)
        {
            _userManager = userManager;
        }

		private async Task OnSave(object obj)
		{
			if (User is null)
				throw new NullReferenceException("User is null");

			Errors = User.Validate().ToList();

			if (((Password != null &&
				Password.Length > 0) ||
				ConfirmedPassword != null &&
				ConfirmedPassword.Length > 0))
			{
				AttributeValidator.GetOnlyErrors(this).ToList().ForEach(x => Errors.Add(x));
				User.Password = Password.GetStringValue();
				OnPropertyChanged(nameof(Errors));
			}


			if (Errors.Any())
				return;

			var response = await _userManager.Update(User);

			if (!response.IsSuccess)
			{
				if (response.Message.Equals("Wrong current password", StringComparison.OrdinalIgnoreCase))
					Errors.Add(l10n.UserView.Errors.WrongCurrentPassword);
				else
					Errors.Add(l10n.Shared.Errors.InternalAppError);

				OnPropertyChanged(nameof(Errors));
			}
		}

        public override Task Initialize()
        {
            SaveCommand = new RelayCommandAsync(OnSave);
            return Task.FromResult(0);
        }
    }
}
