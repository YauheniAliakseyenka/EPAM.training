using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
	[Table(name: "User")]
	public class User
	{
		[Column(name: "Id")]
		public string Id { get; set; }
		[Column(name: "UserName")]
		public string UserName { get; set; }
		[Column(name: "PasswordHash")]
		public string PasswordHash { get; set; }
		[Column(name: "Email")]
		public string Email { get; set; }
		[Column(name: "Firstname")]
		public string Firstname { get; set; }
		[Column(name: "Surname")]
		public string Surname { get; set; }
        [Column(name: "Culture")]
        public string Culture { get; set; }
        [Column(name: "Timezone")]
        public string Timezone { get; set; }
		[Column(name: "Amount")]
		public decimal Amount { get; set; }
	}
}
