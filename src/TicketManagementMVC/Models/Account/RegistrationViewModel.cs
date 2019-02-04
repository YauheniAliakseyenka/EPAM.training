using System.ComponentModel.DataAnnotations;

namespace TicketManagementMVC.Models
{
	public class RegistrationViewModel
	{
		[Required(ErrorMessageResourceName = "PropertyRequired", ErrorMessageResourceType = typeof(ProjectResources.ResourceErrors))]
		[Display(Name = "Username", ResourceType = typeof(ProjectResources.AccountResource))]
		public string UserName { get; set; }

		[Display(Name = "Firstname", ResourceType = typeof(ProjectResources.AccountResource))]
		public string Firstname { get; set; }

		[Display(Name = "Surname", ResourceType = typeof(ProjectResources.AccountResource))]
		public string Surname { get; set; }

		[Display(Name = "SelectLanguage", ResourceType = typeof(ProjectResources.AccountResource))]
		public string Culture { get; set; }

		[Display(Name = "SelectTimezone", ResourceType = typeof(ProjectResources.AccountResource))]
		public string SelectedTimezone { get; set; }

		[Required(ErrorMessageResourceName = "PropertyRequired", ErrorMessageResourceType = typeof(ProjectResources.ResourceErrors))]
        [RegularExpression(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                         + "@"
                         + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$",
            ErrorMessageResourceName = "EmailError", ErrorMessageResourceType = typeof(ProjectResources.ResourceErrors))]
        [Display(Name = "Email", ResourceType = typeof(ProjectResources.AccountResource))]
        public string Email { get; set; }

		[Required(ErrorMessageResourceName = "PropertyRequired", ErrorMessageResourceType = typeof(ProjectResources.ResourceErrors))]
		[DataType(DataType.Password)]
		[Display(Name = "Password", ResourceType = typeof(ProjectResources.AccountResource))]
		[MinLength(10, ErrorMessageResourceName = "PasswordRangeError", ErrorMessageResourceType = typeof(ProjectResources.ResourceErrors))]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "ConfirmPassword", ResourceType = typeof(ProjectResources.AccountResource))]
		[Compare("Password", ErrorMessageResourceName = "ConfirmPasswordError", ErrorMessageResourceType = typeof(ProjectResources.ResourceErrors))]
		public string ConfirmPassword { get; set; }
	}
}