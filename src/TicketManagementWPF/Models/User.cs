using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using TicketManagementWPF.Infrastructure.Utils.Validation;
using TicketManagementWPF.Infrastructure.Validation.Utils;

namespace TicketManagementWPF.Models
{
	public class User
	{
		public int Id { get; set; }
		[CustomRequired("UsernameLabel", typeof(l10n.UserView.View))]
		public string UserName { get; set; }
		[CustomRequired("EmailLabel", typeof(l10n.UserView.View))]
		[RegularExpression(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
						 + "@"
						 + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$",
			 ErrorMessage = "Email is not valid")]
		public string Email { get; set; }
		public string Firstname { get; set; }
		public string Surname { get; set; }
		public string Culture { get; set; }
		public string Timezone { get; set; }
		public string Password { get; set; }

		public ObservableCollection<string> Roles { get; set; }

		public User()
		{
			UserName = string.Empty;
			Email = string.Empty;
			Firstname = string.Empty;
			Surname = string.Empty;
			Culture = "en";
			Timezone = "UTC";
			Password = string.Empty;
			Roles = new ObservableCollection<string>();
		}

		public IEnumerable<string> Validate()
		{
			return AttributeValidator.GetOnlyErrors(this);
		}
	}
}
