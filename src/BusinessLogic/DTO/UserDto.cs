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
	}
}
