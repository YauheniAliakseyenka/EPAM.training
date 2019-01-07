using BusinessLogic.DTO;
using BusinessLogic.Exceptions;
using DataAccess;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BusinessLogic.Services.UserServices
{
	internal partial class UserService : IUserService
	{
		private readonly IWorkUnit _context;

		public UserService(IWorkUnit context)
		{
			_context = context;
		}

		public void Create(UserDto entity)
		{
            if (entity == null)
                throw new ArgumentNullException();

			var addUser = mapToUser(entity);
			using (var transacrion = _context.CreateTransaction())
			{
				try
				{
					_context.UserRepository.Create(addUser);
					_context.Save();

					var userRole = (from roles in _context.RoleRepository.GetList()
									where roles.Name.Equals("user", StringComparison.OrdinalIgnoreCase)
									select roles).FirstOrDefault();

					if (userRole == null)
						throw new UserException("Role User does not exists");

					_context.UserRoleRepository.Create(new UserRole
					{
						RoleId = userRole.Id, 
						UserId = addUser.Id
					});
					_context.Save();
					transacrion.Commit();
				}
				catch(Exception)
				{
					transacrion.Rollback();
                    throw;
				}
			}
		}

		public IEnumerable<UserDto> FindBy(Expression<Func<UserDto, bool>> expression)
		{
			var result = new List<UserDto>();
			Expression<Func<User, bool>> predicate = x => expression.Compile().Invoke(mapToUserDto(x));
			var list = _context.UserRepository.FindBy(predicate);

			if (!list.Any())
				return result;

			foreach (var item in list)
			{
				result.Add(mapToUserDto(item));
			}

			return result;
		}

		public void Update(UserDto entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			var update = _context.UserRepository.FindBy(x => x.UserName.Equals(entity.UserName)).FirstOrDefault();
			update.Surname = entity.Surname;
			update.Firstname = entity.Firstname;
			update.Culture = entity.Culture;
			update.Timezone = entity.Timezone;
			update.PasswordHash = entity.PasswordHash;
			update.Amount = entity.Amount;
			update.Email = entity.Email;
			_context.UserRepository.Update(update);
			_context.Save();
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

		public void Delete(string id)
		{
			_context.UserRepository.Delete(id);
			_context.Save();
		}

		public UserDto Get(string id)
		{
			var user = _context.UserRepository.Get(id);

			return user == null ? null : mapToUserDto(user);
		}

		public IEnumerable<UserDto> GetList()
		{
			return _context.UserRepository.GetList().Select(x => mapToUserDto(x)).ToList();
		}
	}
}
