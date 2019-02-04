using System.ComponentModel.DataAnnotations;

namespace TicketManagementMVC.Models
{
	public class UserProfileViewModel
	{
		[Required(ErrorMessageResourceName = "PropertyRequired", ErrorMessageResourceType = typeof(ProjectResources.ResourceErrors))]
		[EmailAddress(ErrorMessageResourceType = typeof(ProjectResources.ResourceErrors), ErrorMessageResourceName = "EmailError")]
		[Display(Name = "Email", ResourceType = typeof(ProjectResources.AccountResource))]
		public string Email { get; set; }

		[Display(Name = "Firstname", ResourceType = typeof(ProjectResources.AccountResource))]
		public string Firstname { get; set; }

		[Display(Name = "Surname", ResourceType = typeof(ProjectResources.AccountResource))]
		public string Surname { get; set; }

		[Display(Name = "SelectLanguage", ResourceType = typeof(ProjectResources.AccountResource))]
		public string Culture { get; set; }

		[Display(Name = "SelectTimezone", ResourceType = typeof(ProjectResources.AccountResource))]
		public string Timezone { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Password", ResourceType = typeof(ProjectResources.AccountResource))]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "NewPassword", ResourceType = typeof(ProjectResources.AccountResource))]
		[MinLength(10, ErrorMessageResourceName = "PasswordRangeError", ErrorMessageResourceType = typeof(ProjectResources.ResourceErrors))]
		public string NewPassword { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "ConfirmNewPassword", ResourceType = typeof(ProjectResources.AccountResource))]
		[Compare("NewPassword", ErrorMessageResourceName = "ConfirmPasswordError", ErrorMessageResourceType = typeof(ProjectResources.ResourceErrors))]
		public string ConfirmNewPassword { get; set; }
	}
}