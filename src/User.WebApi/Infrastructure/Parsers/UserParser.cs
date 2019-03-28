using BusinessLogic.DTO;
using User.WebApi.Infrastructure.AuthManager;
using User.WebApi.Models;

namespace User.WebApi.Infrastructure.Parsers
{
	internal class UserParser
	{
		public static UserDto CreateUpdateUserModelToUserDto(CreateUpdateUserModel from)
		{
            var hashed = PasswordHasher.ComputeHash(from.Password);

            return new UserDto
			{
				Email = from.Email,
				Surname = from.Surname,
				Culture = from.Culture,
				Firstname = from.Firstname,
				Timezone = from.Timezone,
				PasswordHash = hashed?.Hash,
				UserName = from.UserName,
				Salt = hashed?.Salt
            };
		}

        public static UserDto CreateUserModelToUserDto(CreateUserModel from)
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

        public static UserModel UserDtoToUserModel(UserDto from)
		{
			return new UserModel
			{
				Id = from.Id,
				Email = from.Email,
				Surname = from.Surname,
				Culture = from.Culture,
				Firstname = from.Firstname,
				Timezone = from.Timezone,
				UserName = from.UserName,
				Amount = from.Amount
			};
		}
	}
}
