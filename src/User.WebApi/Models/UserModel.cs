using System.ComponentModel.DataAnnotations;

namespace User.WebApi.Models
{
	public class UserModel
	{
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

		public string Firstname { get; set; }

        [Required]
		[RegularExpression(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
						 + "@"
						 + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$",
			 ErrorMessage = "Email is not valid")]
		public string Email { get; set; }

		public string Surname { get; set; }

        [Required]
        public string Culture { get; set; }

        [Required]
        public string Timezone { get; set; }

		public decimal Amount { get; set; }

        [Required]
		[MinLength(10, ErrorMessage = "Password must be more than 10 characters")]
		public string Password { get; set; }
	}
}
