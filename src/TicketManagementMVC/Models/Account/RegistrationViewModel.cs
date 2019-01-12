using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;

namespace TicketManagementMVC.Models
{
	public class RegistrationViewModel
	{
		[Required(ErrorMessageResourceName = "PropertyRequired", ErrorMessageResourceType = typeof(I18N.ResourceErrors))]
		[Display(Name = "Username", ResourceType = typeof(I18N.Resource))]
		public string UserName { get; set; }

		[Display(Name = "Firstname", ResourceType = typeof(I18N.Resource))]
		public string Firstname { get; set; }

		[Display(Name = "Surname", ResourceType = typeof(I18N.Resource))]
		public string Surname { get; set; }

		[Display(Name = "SelectLanguage", ResourceType = typeof(I18N.Resource))]
		public string Culture { get; set; }

		[Display(Name = "SelectTimezone", ResourceType = typeof(I18N.Resource))]
		public string SelectedTimezone { get; set; }

		[Required(ErrorMessageResourceName = "PropertyRequired", ErrorMessageResourceType = typeof(I18N.ResourceErrors))]
		[EmailAddress(ErrorMessageResourceType = typeof(I18N.ResourceErrors), ErrorMessageResourceName = "EmailError")]
		[Display(Name = "Email", ResourceType = typeof(I18N.Resource))]
		public string Email { get; set; }

		[Required(ErrorMessageResourceName = "PropertyRequired", ErrorMessageResourceType = typeof(I18N.ResourceErrors))]
		[DataType(DataType.Password)]
		[Display(Name = "Password", ResourceType = typeof(I18N.Resource))]
		[MinLength(10, ErrorMessageResourceName = "PasswordRangeError", ErrorMessageResourceType = typeof(I18N.ResourceErrors))]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "ConfirmPassword", ResourceType = typeof(I18N.Resource))]
		[Compare("Password", ErrorMessageResourceName = "ConfirmPasswordError", ErrorMessageResourceType = typeof(I18N.ResourceErrors))]
		public string ConfirmPassword { get; set; }
	}

	public enum Culture
	{
		[Display(Name = "English", ResourceType = typeof(I18N.Resource))]
		en = 1,
		[Display(Name = "Russian", ResourceType = typeof(I18N.Resource))]
		ru,
		[Display(Name = "Belarusian", ResourceType = typeof(I18N.Resource))]
		be
	}

	public class CultureEnum
	{
		public static string GetDescription(Enum en)
		{
			Type type = en.GetType();
			MemberInfo[] memInfo = type.GetMember(en.ToString());
			if (memInfo != null && memInfo.Length > 0)
			{
				object[] attrs = memInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);
				if (attrs != null && attrs.Length > 0)
					return I18N.Resource.ResourceManager.GetString(((DisplayAttribute)attrs[0]).Name);
			}
			return en.ToString();
		}
	}
}