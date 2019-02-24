using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace TicketManagementMVC.Infrastructure.Authentication
{
    public class User : IUser<int>
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
        [JsonProperty("UserName")]
        public string UserName { get; set; }
		[JsonProperty("Password")]
		public string Password { get; set; }
		[JsonProperty("NewPassword")]
		public string NewPassword { get; set; }
		[JsonProperty("Email")]
        public string Email { get; set; }
        [JsonProperty("Firstname")]
        public string Firstname { get; set; }
        [JsonProperty("Surname")]
        public string Surname { get; set; }
        [JsonProperty("Culture")]
        public string Culture { get; set; }
        [JsonProperty("Timezone")]
        public string Timezone { get; set; }
        [JsonProperty("Amount")]
        public decimal Amount { get; set; }
	}
}