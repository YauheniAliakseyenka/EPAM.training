using BusinessLogic.BusinessModels;
using BusinessLogic.Services.UserServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
	public interface IEmailService
	{
		void Send(object sender, EmailEventArgs args);
	}
}
