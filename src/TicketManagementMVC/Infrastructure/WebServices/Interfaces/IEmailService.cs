using BusinessLogic.BusinessModels;

namespace TicketManagementMVC.Infrastructure.WebServices.Interfaces
{
	public interface IEmailService
	{
		void Send(object sender, OrderEventArgs args);
	}
}
