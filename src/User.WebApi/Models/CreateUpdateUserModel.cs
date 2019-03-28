using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace User.WebApi.Models
{
	public class CreateUpdateUserModel
	{
		[Required]
        [MinLength(10, ErrorMessage = "Password must be more than 10 characters")]
        public string UserName { get; set; }
		public string Firstname { get; set; }
		public string Surname { get; set; }
		[Required]
        [RegularExpression(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                         + "@"
                         + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$",
             ErrorMessage = "Email is not valid")]
        public string Email { get; set; }
		public string Culture { get; set; }
		public string Timezone { get; set; }
		public List<string> Roles { get; set; }
		[MinLength(10)]
		public string Password { get; set; }
	}
}
