using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace User.WebApi.Models
{
	public class UserModel
	{
		public int Id { get; set; }
		public string UserName { get; set; }
		public string Firstname { get; set; }
		public string Surname { get; set; }
		public string Email { get; set; }
		public string Culture { get; set; }
		public string Timezone { get; set; }
		public decimal Amount { get; set; }
        public List<string> Roles { get; set; }
	}
}
