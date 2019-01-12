using System;

namespace BusinessLogic.DTO
{
	public class UserDto
	{
		public string Id { get; set; }
		public string UserName { get; set; }
		public string PasswordHash { get; set; }
		public string Email { get; set; }
		public string Firstname { get; set; }
		public string Surname { get; set; }
		public string Culture { get; set; }
		public string Timezone { get; set; }
		public decimal Amount { get; set; }

        public override bool Equals(object obj)
        {
            var entity = obj as UserDto;

            if (entity == null)
                return false;

			if (Id.Equals(entity.Id, StringComparison.Ordinal) &&
                UserName.Equals(entity.UserName, StringComparison.Ordinal) &&
                PasswordHash.Equals(entity.PasswordHash, StringComparison.Ordinal) &&
                Email.Equals(entity.Email, StringComparison.Ordinal) &&
                Firstname.Equals(entity.Firstname, StringComparison.OrdinalIgnoreCase) &&
                Surname.Equals(entity.Surname, StringComparison.OrdinalIgnoreCase) &&
                Culture.Equals(entity.Culture, StringComparison.OrdinalIgnoreCase) &&
                Timezone.Equals(entity.Timezone, StringComparison.OrdinalIgnoreCase) &&
                Amount == entity.Amount)
                return true;

            return false;
        }

        public override int GetHashCode() => (Id, UserName, PasswordHash, Email, Firstname, Surname, Culture, Timezone, Amount).GetHashCode();
    }
}
