using System.ComponentModel.DataAnnotations;

namespace User.WebApi.Models
{
	public class UpdateUserProfileModel
    {
		public string Firstname { get; set; }
		public string Surname { get; set; }

        [Required]
		public string Culture { get; set; }

        [Required]
        public string Timezone { get; set; }

        public string Password { get; set; }

		[MinLength(10, ErrorMessage = "Password must be more than 10 characters")]
		public string NewPassword { get; set; }
	}
}
