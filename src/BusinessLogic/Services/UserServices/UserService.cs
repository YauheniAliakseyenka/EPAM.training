using BusinessLogic.DTO;
using BusinessLogic.Exceptions;
using DataAccess;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

			if(isUserNameTaken(entity.UserName))
				throw new UserException("Username is already taken");

			if (isEmailTaken(entity.Email))
				throw new UserException("Email is already taken");

			var addUser = mapToUser(entity);
			using (var transacrion = _context.CreateTransaction())
			{
				try
				{
					_context.UserRepository.Create(addUser);
					await _context.SaveAsync();

					var role = _context.RoleRepository.FindBy(x => x.Name.Equals("user", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

					if (role == null)
						throw new UserException("Role User does not exists");

					_context.UserRoleRepository.Create(new UserRole
					{
						RoleId = role.Id, 
						UserId = addUser.Id
					});
					await _context.SaveAsync();
					transacrion.Commit();
				}
				catch(Exception)
				{
					transacrion.Rollback();
                    throw;
				}
			}
		}

		public Task<IEnumerable<UserDto>> FindBy(Expression<Func<UserDto, bool>> expression)
		{
			Expression<Func<User, bool>> predicate = x => expression.Compile().Invoke(mapToUserDto(x));
			var list = _context.UserRepository.FindBy(predicate);

			return Task.FromResult(list.Select(x => mapToUserDto(x)).AsEnumerable());
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

		private UserDto mapToUserDto(User from)
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

		private User mapToUser(UserDto from)
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

		public async Task Delete(string id)
		{
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException();

            _context.UserRepository.Delete(id);
			await _context.SaveAsync();
		}

		public async Task<UserDto> Get(string id)
		{
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException();

            var user = await _context.UserRepository.GetAsync(id);

			return user == null ? null : mapToUserDto(user);
		}

		public async Task<IEnumerable<UserDto>> GetList()
		{
			var list = await _context.UserRepository.GetListAsync();

			return list.Select(x => mapToUserDto(x));
		}

		public Task<IEnumerable<string>> GetRoles(string userName)
		{
			var roles = (from user in _context.UserRepository.GetList()
						 join userRole in _context.UserRoleRepository.GetList() on user.Id equals userRole.UserId
						 join role in _context.RoleRepository.GetList() on userRole.RoleId equals role.Id
						 where user.UserName.Equals(userName, StringComparison.Ordinal)
						 select role.Name).ToList();

			return Task.FromResult(roles.AsEnumerable());
		}

		private bool isUserNameTaken(string userName)
		{
			return (from users in _context.UserRepository.GetList() where users.UserName.Equals(userName, StringComparison.Ordinal)
					select users).Any();
		}

		private bool isEmailTaken(string email)
		{
			return (from users in _context.UserRepository.GetList()
					where users.Email.Equals(email, StringComparison.OrdinalIgnoreCase)
					select users).Any();
		}
	}
}
