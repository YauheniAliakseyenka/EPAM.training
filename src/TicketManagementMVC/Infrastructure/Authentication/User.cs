using Microsoft.AspNet.Identity;

namespace TicketManagementMVC.Infrastructure.Authentication
{
    public class User : IUser<int>
    {
        public int Id { get; set; }
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