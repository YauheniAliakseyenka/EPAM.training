using System;
using System.Security.Cryptography;
using User.WebApi.Models;

namespace User.WebApi.Infrastructure.AuthManager
{
	internal class PasswordHasher
	{
		private const int SaltSize = 64;
		private const int PasswordSize = 128;
		private const int IterationsNumber = 10000;

		public static HashedPasswordModel ComputeHash(string password)
		{
            if (string.IsNullOrEmpty(password))
                return null;

			//generate random salt
			byte[] salt;
			new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);
			var saltStringValue = Convert.ToBase64String(salt);

			//hash password
			var passwordBytes = new Rfc2898DeriveBytes(password, salt, IterationsNumber);
			var hashedPassword = Convert.ToBase64String(passwordBytes.GetBytes(PasswordSize));


			return new HashedPasswordModel { Salt = saltStringValue, Hash = hashedPassword };
		}

		public static bool VerifyHash(string passwordToVerify, string salt, string hash)
		{
			var saltBytes = Convert.FromBase64String(salt);
			var passwordBytes = new Rfc2898DeriveBytes(passwordToVerify, saltBytes, IterationsNumber);

			return Convert.ToBase64String(passwordBytes.GetBytes(PasswordSize)).Equals(hash, StringComparison.Ordinal);
		}
	}
}
