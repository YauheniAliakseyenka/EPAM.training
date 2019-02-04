using BusinessLogic.DTO;
using BusinessLogic.Exceptions;
using DataAccess;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Services.UserServices
{
	internal class UserService : IUserService
	{
		private readonly IWorkUnit _context;

		public UserService(IWorkUnit context)
		{
			_context = context;
        }

		public async Task Create(UserDto entity)
		{
            if (entity == null)
                throw new ArgumentNullException();

			if(IsUserNameTaken(entity.UserName))
				throw new UserException("Username is already taken");

			if (IsEmailTaken(entity.Email))
				throw new UserException("Email is already taken");

			var addUser = MapToUser(entity);
			using (var transacrion = _context.CreateTransaction())
			{
				try
				{
					_context.UserRepository.Create(addUser);
					await _context.SaveAsync();

					var findRole = await _context.RoleRepository.FindByAsync(x => x.Name.Equals("user", StringComparison.OrdinalIgnoreCase));
					var role = findRole.FirstOrDefault();

					if (role == null)
						throw new UserException("Role User does not exists");

					_context.UserRoleRepository.Create(new UserRole
					{
						RoleId = role.Id, 
						UserId = addUser.Id
					});
					await _context.SaveAsync();
					transacrion.Commit();
					entity.Id = addUser.Id;
				}
				catch(Exception)
				{
					transacrion.Rollback();
                    throw;
				}
			}
		}

		public async Task Update(UserDto entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			var update = await _context.UserRepository.GetAsync(entity.Id);

            if (update == null)
                throw new UserException("User does not exists");

            update.Surname = entity.Surname;
			update.Firstname = entity.Firstname;
			update.Culture = entity.Culture;
			update.Timezone = entity.Timezone;
			update.PasswordHash = entity.PasswordHash;
			update.Amount = entity.Amount;
			update.Email = entity.Email;
			_context.UserRepository.Update(update);

			await _context.SaveAsync();
		}

		private UserDto MapToUserDto(User from)
		{
			return new UserDto
			{
				UserName = from.UserName,
				Timezone = from.Timezone,
				Surname = from.Surname,
				Amount = from.Amount,
				Culture = from.Culture,
				Email = from.Email,
				Firstname = from.Firstname,
				Id = from.Id,
				PasswordHash = from.PasswordHash
			};
		}

		private User MapToUser(UserDto from)
		{
			return new User
			{
				UserName = from.UserName,
				Timezone = from.Timezone,
				Surname = from.Surname,
				Amount = from.Amount,
				Culture = from.Culture,
				Email = from.Email,
				Firstname = from.Firstname,
				Id = from.Id,
				PasswordHash = from.PasswordHash
			};
		}

		public async Task Delete(int id)
		{
			var delete = await _context.UserRepository.GetAsync(id);

			if (delete == null)
				return;

            _context.UserRepository.Delete(delete);
			await _context.SaveAsync();
		}

		public async Task<UserDto> Get(int id)
		{
			var user = await _context.UserRepository.GetAsync(id);

			return user == null ? null : MapToUserDto(user);
		}

		public async Task<IEnumerable<UserDto>> GetList()
		{
			var list = await _context.UserRepository.GetListAsync();

			return list.Select(x => MapToUserDto(x)).ToList();
		}

        /// <summary>
        /// Get roles of user by username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
		public Task<IEnumerable<string>> GetRoles(string userName)
		{
			var roles = (from user in _context.UserRepository.GetList()
						 join userRole in _context.UserRoleRepository.GetList() on user.Id equals userRole.UserId
						 join role in _context.RoleRepository.GetList() on userRole.RoleId equals role.Id
						 where user.UserName.Equals(userName, StringComparison.Ordinal)
						 select role.Name).ToList();

			return Task.FromResult(roles.AsEnumerable());
		}

		private bool IsUserNameTaken(string userName)
		{
			return (from users in _context.UserRepository.GetList()
					where users.UserName.Equals(userName, StringComparison.Ordinal)
					select users).Any();
		}

        private bool IsEmailTaken(string email)
		{
			return (from users in _context.UserRepository.GetList()
					where users.Email.Equals(email, StringComparison.OrdinalIgnoreCase)
					select users).Any();
		}

		public Task<UserDto> FindByEmail(string email)
		{
			var user = (from users in _context.UserRepository.GetList()
						where users.Email.Equals(email, StringComparison.OrdinalIgnoreCase)
						select users).FirstOrDefault();
			var result = user == null ? null : MapToUserDto(user);

			return Task.FromResult(result);
		}

		public Task<UserDto> FindByUserName(string userName)
		{
			var user = (from users in _context.UserRepository.GetList()
						where users.UserName.Equals(userName, StringComparison.Ordinal)
						select users).FirstOrDefault();
			var result = user == null ? null : MapToUserDto(user);

			return Task.FromResult(result);
		}
	}
}
