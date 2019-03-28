using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TicketManagementWPF.Helpers;
using TicketManagementWPF.Helpers.WindowManagement;
using TicketManagementWPF.Infrastructure;
using TicketManagementWPF.Infrastructure.Commands;
using TicketManagementWPF.Infrastructure.Utils.UserManagement;

namespace TicketManagementWPF.ViewModels
{
	internal class UserManagementViewModel : ViewModelAbstract
	{
		public const string UserSavedOperationKey = "UserSaved";

		#region Commands

		public ICommand CreateUserCommand { get; private set; }
		public ICommand EditUserCommand { get; private set; }
		public ICommand DeleteUserCommand { get; private set; }
		public ICommand FindCommand { get; private set; }
		public ICommand ResetCommand { get; private set; }

		#endregion

		#region Binding properties

		private ObservableCollection<Models.User> _userList;
		public ObservableCollection<Models.User> UserList
		{
			get { return _userList; }
			set
			{
				_userList = value;
				OnPropertyChanged();
			}
		}

		private string _searchBy;
		public string SearchBy
		{
			get { return _searchBy; }
			set
			{
				_searchBy = value;
				OnPropertyChanged();
			}
		}

		private string _searchValue;
		public string SearchValue
		{
			get { return _searchValue; }
			set
			{
				_searchValue = value;
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

		private Models.User _markedUser;

		private readonly IWindowHelper _windowHelper;
		private readonly IMediator _mediator;
		private readonly IUserManagement _userManager;

		public UserManagementViewModel(
			IWindowHelper windowHelper, 
			IMediator mediator, 
			IUserManagement userManager)
		{
			_windowHelper = windowHelper;
			_mediator = mediator;
			_userManager = userManager;
		}

		public override async Task Initialize()
		{
			await LoadUserList();
			_mediator.Subscribe(UserSavedOperationKey, UserSaved);
			EditUserCommand = new RelayCommandAsync(OnEditUser);
			CreateUserCommand = new RelayCommandAsync(OnCreateUser);
			DeleteUserCommand = new RelayCommandAsync(OnDeleteUser);
			FindCommand = new RelayCommandAsync(OnFind);
			ResetCommand = new RelayCommandAsync(OnReset);
		}

		private async Task OnEditUser(object obj)
		{
			if (!(obj is Models.User user))
				return;

			var result = await _userManager.Get(user.Id);

			if (!result.IsSuccess)
				throw new Exception("User loading error");

			_markedUser = user;
            await OpenUserWindow(result.Object as Models.User,user.UserName);
			_markedUser = null;
        }

		private async Task OnCreateUser(object obj)
		{
			await OpenUserWindow(new Models.User(), string.Empty);
        }

        private async Task OpenUserWindow(Models.User user, string title)
        {
			DisplayError(string.Empty);
			var viewModel = await _windowHelper.GetViewModel<UserViewModel>() as UserViewModel;
            viewModel.User = user;
			viewModel.Title = title;
            _windowHelper.ShowDialog(viewModel);
        }

        private async Task OnDeleteUser(object obj)
		{
			DisplayError(string.Empty);

			if (!(obj is Models.User user))
				return;

			var result = await _userManager.Delete(user);

			if (!result.IsSuccess)
			{
				DisplayError(l10n.Shared.Errors.InternalAppError);
				return;
			}

			UserList.Remove(user);
		}

		private void UserSaved(object obj)
		{
			DisplayError(string.Empty);

			if (!(obj is Models.User user))
				return;

			if (_markedUser is null)
			{
				var newUser = new Models.User
				{
					UserName = user.UserName,
					Email = user.Email
				};
				UserList.Add(newUser);
				return;
			}

			var index = UserList.IndexOf(_markedUser);
			UserList.Remove(_markedUser);

			_markedUser.UserName = user.UserName;
			_markedUser.Email = user.Email;
			UserList.Insert(index, _markedUser);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposed)
				return;

			if (disposing)
			{
				_mediator.Subscribe(UserSavedOperationKey, UserSaved);
			}

			disposed = true;
		}

		private async Task OnFind(object obj)
		{
			if (string.IsNullOrEmpty(SearchValue) || string.IsNullOrEmpty(SearchBy))
				return;

			var enumValue = Enum.GetValues(typeof(UserSearchEnum)).Cast<UserSearchEnum>().
				SingleOrDefault(x => EnumHelper.GetDescription(x).Equals(SearchBy, StringComparison.OrdinalIgnoreCase));

			var result = await _userManager.FindBy(enumValue, SearchValue);
			var user = result.Object as Models.User;

			if (user is null)
				UserList = null;
			else
				UserList = new ObservableCollection<Models.User>
				{
					user
				};
		}

		private async Task OnReset(object obj)
		{
			await LoadUserList();
			SearchValue = null;
		}

		private async Task LoadUserList()
		{
			var result = await _userManager.ToList();

			if (!result.IsSuccess)
				throw new Exception("User's list loading error");

			UserList = (result.Object as UserList).Users;
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
