using System.ComponentModel.DataAnnotations;

namespace User.WebApi.Models
{
	public class UpdateUserModel
    {
		public string Firstname { get; set; }
		public string Surname { get; set; }

        [Required]
		public string Culture { get; set; }

        [Required]
        public string Timezone { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public string Password { get; set; }
        public string NewPassword { get; set; }
	}
}
