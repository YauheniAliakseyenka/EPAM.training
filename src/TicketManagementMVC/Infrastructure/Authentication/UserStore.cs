using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using BusinessLogic.Services;
using BusinessLogic.DTO;
using BusinessLogic.Exceptions;

namespace TicketManagementMVC.Infrastructure.Authentication
{
    internal class UserStore : IUserClaimStore<User>, IUserPasswordStore<User>, IUserEmailStore<User>
    {
		bool disposed = false;
		SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
		private IUserService _userService;

        public UserStore(IUserService userService)
        {
            _userService = userService;
        }

        public Task AddClaimAsync(User user, Claim claim)
        {
			throw new NotImplementedException();
        }

        public Task CreateAsync(User user)
        {
            var userModel = maptToUserDto(user);
            userModel.Id = Guid.NewGuid().ToString();
			_userService.Create(userModel);

            return Task.FromResult(0);
        }

        public Task DeleteAsync(User user)
        {
			 _userService.Delete(user.Id);

			return Task.FromResult(0);
        }

        public Task<User> FindByIdAsync(string userId)
        {
            var user = _userService.Get(userId);

            var result = user != null ? mapToUser(user) : null;

            return Task.FromResult(result);
        }

        public Task<User> FindByNameAsync(string userName)
        {
            var user = _userService.FindBy(x => x.UserName.Equals(userName)).FirstOrDefault();

            var result = user != null ? mapToUser(user) : null;

            return Task.FromResult(result);
        }

        public Task<IList<Claim>> GetClaimsAsync(User user)
        {
			var userRoles = _userService.GetRoles(user.UserName);
			IList<Claim> claims = new List<Claim>();

			userRoles.ToList().ForEach(x =>
            {
                claims.Add(new Claim(ClaimTypes.Role, x));
            });
			
            return Task.FromResult(claims);
        }

        public Task<string> GetPasswordHashAsync(User user)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            var userModel = _userService.Get(user.Id);

            return Task.FromResult(userModel.PasswordHash != null);
        }

        public Task RemoveClaimAsync(User user, Claim claim)
        {
            throw new NotImplementedException();
        }

        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            user.PasswordHash = passwordHash;

            return Task.FromResult(0);
        }

        public Task UpdateAsync(User user)
        {
			var updateUser = _userService.Get(user.Id);

			if(updateUser != null)
			{
				updateUser.Surname = user.Surname;
				updateUser.Firstname = user.Firstname;
				updateUser.Email = user.Email;
				updateUser.PasswordHash = user.PasswordHash;
				updateUser.Amount = user.Amount;
				updateUser.Culture = user.Culture;
				updateUser.Email = user.Email;
				updateUser.Timezone = user.Timezone;
			}

			 _userService.Update(updateUser);

			return Task.FromResult(0);
        }

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
				return;

			if(disposing)
			{
				handle.Dispose();
			}

			disposed = true;
		}

		private User mapToUser(UserDto user)
        {
            return new User
			{
                Surname = user.Surname,
                UserName = user.UserName,
                Email = user.Email,
                Firstname = user.Firstname,
                Id = user.Id,
                PasswordHash = user.PasswordHash,
				Culture = user.Culture,
				Amount = user.Amount,
				Timezone = user.Timezone
            };
        }

        private UserDto maptToUserDto(User user)
        {
            return new UserDto
			{
                Surname = user.Surname,
                UserName = user.UserName,
                Email = user.Email,
                Firstname = user.Firstname,
                Id = user.Id,
                PasswordHash = user.PasswordHash,
				Culture = user.Culture,
				Timezone = user.Timezone,
				Amount = user.Amount
			};
        }

		public Task SetEmailAsync(User user, string email)
		{
			user.Email = email;

			return Task.FromResult(0);
		}

		public Task<string> GetEmailAsync(User user)
		{
			return Task.FromResult(user.Email);
		}

		public Task<bool> GetEmailConfirmedAsync(User user)
		{
			throw new NotImplementedException();
		}

		public Task SetEmailConfirmedAsync(User user, bool confirmed)
		{
			throw new NotImplementedException();
		}

		public Task<User> FindByEmailAsync(string email)
		{
			var user = _userService.FindBy(x => x.Email.Equals(email)).FirstOrDefault();

			var result = user != null ? mapToUser(user) : null;

			return Task.FromResult(result);
		}
	}
}