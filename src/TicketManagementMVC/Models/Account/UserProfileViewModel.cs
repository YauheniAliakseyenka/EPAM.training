using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TicketManagementMVC.Models
{
	public class UserProfileViewModel
	{
		[Required(ErrorMessageResourceName = "PropertyRequired", ErrorMessageResourceType = typeof(I18N.ResourceErrors))]
		[EmailAddress(ErrorMessageResourceType = typeof(I18N.ResourceErrors), ErrorMessageResourceName = "EmailError")]
		[Display(Name = "Email", ResourceType = typeof(I18N.Resource))]
		public string Email { get; set; }

		[Display(Name = "Firstname", ResourceType = typeof(I18N.Resource))]
		public string Firstname { get; set; }

		[Display(Name = "Surname", ResourceType = typeof(I18N.Resource))]
		public string Surname { get; set; }

		[Display(Name = "SelectLanguage", ResourceType = typeof(I18N.Resource))]
		public string Culture { get; set; }

		[Display(Name = "SelectTimezone", ResourceType = typeof(I18N.Resource))]
		public string Timezone { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Password", ResourceType = typeof(I18N.Resource))]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "NewPassword", ResourceType = typeof(I18N.Resource))]
		[MinLength(10, ErrorMessageResourceName = "PasswordRangeError", ErrorMessageResourceType = typeof(I18N.ResourceErrors))]
		public string NewPassword { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "ConfirmNewPassword", ResourceType = typeof(I18N.Resource))]
		[Compare("NewPassword", ErrorMessageResourceName = "ConfirmPasswordError", ErrorMessageResourceType = typeof(I18N.ResourceErrors))]
		public string ConfirmNewPassword { get; set; }
	}
}