using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Input;
using TicketManagementWPF.Helpers;
using TicketManagementWPF.Helpers.WindowManagement;
using TicketManagementWPF.Infrastructure;
using TicketManagementWPF.Infrastructure.Commands;
using TicketManagementWPF.Infrastructure.Utils.UserManagement;
using TicketManagementWPF.Infrastructure.Utils.Validation;
using TicketManagementWPF.Infrastructure.Validation.Utils;
using TicketManagementWPF.Infrastructure.Extensions;
using User.WebApi.Helper;

namespace TicketManagementWPF.ViewModels
{
	internal class UserViewModel : ViewModelAbstract, IDisplayWindow
	{
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
				_displayView = value;
				OnPropertyChanged();
			}
		}

		private string _title;
		public string Title
		{
			get { return _title; }
			set
			{
				_title = value;
				OnPropertyChanged();
			}
		}

		private Models.User _user;
		public Models.User User
		{
			get { return _user; }
			set
			{
				_user = value;
				OnUserChanged(_user);
				OnPropertyChanged();
			}
		}

		private ObservableCollection<string> _roleList;
		public ObservableCollection<string> RoleList
		{
			get { return _roleList; }
			set
			{
				_roleList = value;
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
		[CustomRequired("PasswordLabel", typeof(l10n.UserView.View))]
		[MinLength(10, ErrorMessageResourceName = "PasswordLength", ErrorMessageResourceType = typeof(l10n.UserView.Errors))]
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
		[PasswordCompare(nameof(Password), ErrorMessageResourceName = "ConfirmPassword", ErrorMessageResourceType = typeof(l10n.UserView.Errors))]
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

		private readonly IUserManagement _userManager;
		private readonly IMediator _mediator;

		public UserViewModel(IUserManagement userManager, IMediator mediator)
		{
			_userManager = userManager;
			_mediator = mediator;
		}

		private async Task OnSave(object obj)
		{
			Errors = User.Validate().ToList();

			CheckPassword();

			if (Errors.Any())
				return;

			User.Password = Password?.GetStringValue();

			ResponseModel response;
			if (User.Id <= 0)
				response = await _userManager.Create(User);
			else
				response = await _userManager.Update(User);

			if (!response.IsSuccess)
			{
				if (response.Message.Equals("Username is already taken", StringComparison.OrdinalIgnoreCase))
					Errors.Add(l10n.UserView.Errors.UsernameIsTaken);
				else
				if (response.Message.Equals("Username is already taken", StringComparison.OrdinalIgnoreCase))
					Errors.Add(l10n.UserView.Errors.EmailIsTaken);
                else
					Errors.Add(l10n.Shared.Errors.InternalAppError);

                OnPropertyChanged(nameof(Errors));
				return;
			}

			_mediator.Raise(UserManagementViewModel.UserSavedOperationKey, User);
		}

		public override Task Initialize()
		{
			DisplayView = this;
			RoleList = new ObservableCollection<string>();
			SaveCommand = new RelayCommandAsync(OnSave);

			return Task.FromResult(0);
		}

		private void OnUserChanged(Models.User user)
		{
			var roleDescriptionList = Enum.GetValues(typeof(Role)).Cast<Role>().Select(x => EnumHelper.GetDescription(x)).ToList();

			foreach (var role in roleDescriptionList)
				if (!User.Roles.Any(x => x.Equals(role, StringComparison.OrdinalIgnoreCase)))
					RoleList.Add(role);

			var userRole = RoleList.SingleOrDefault(x => x.Equals("user", StringComparison.OrdinalIgnoreCase));
			
			if(userRole != null)
			{
				RoleList.Remove(userRole);
				User.Roles.Add(userRole);
			}
		}

		private void CheckPassword()
		{
			//if a new user
			if (User.Id <= 0)
			{
				AttributeValidator.GetOnlyErrors(this).ToList().ForEach(x => Errors.Add(x));
				OnPropertyChanged(nameof(Errors));
			}
			else
			{
				if (((Password != null &&
				Password.Length > 0) ||
				ConfirmedPassword != null &&
				ConfirmedPassword.Length > 0))
				{
					AttributeValidator.GetOnlyErrors(this).ToList().ForEach(x => Errors.Add(x));
					OnPropertyChanged(nameof(Errors));
				}
			}

		}
	}
}
