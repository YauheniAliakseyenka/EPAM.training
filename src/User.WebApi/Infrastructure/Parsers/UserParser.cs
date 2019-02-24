using BusinessLogic.DTO;
using User.WebApi.Infrastructure.AuthManager;
using User.WebApi.Models;

namespace User.WebApi.Infrastructure.Parsers
{
	internal class UserParser
	{
		public static UserModel UserDtoToUserModel(UserDto from)
		{
			return new UserModel
			{
				UserName = from.UserName,
				Surname = from.Surname,
				Amount = from.Amount,
				Culture = from.Culture,
				Email = from.Email,
				Firstname = from.Firstname,
				Timezone = from.Timezone
			};
		}

		public static UserDto UserModelToUserDto(UserModel from)
		{
			return new UserDto
			{
				Email = from.Email,
				Surname = from.Surname,
				Amount = from.Amount,
				Culture = from.Culture,
				Firstname = from.Firstname,
				Timezone = from.Timezone,
                UserName = from.UserName
			};
		}

		public static UserDto RegistratationUserModelToUserDto(RegistratationUserModel from)
		{
            var hashed = PasswordHasher.ComputeHash(from.Password);

            return new UserDto
			{
				Email = from.Email,
				Surname = from.Surname,
				Culture = from.Culture,
				Firstname = from.Firstname,
				Timezone = from.Timezone,
				PasswordHash = hashed.Hash,
				UserName = from.UserName,
				Salt = hashed.Salt
            };
		}
	}
}
