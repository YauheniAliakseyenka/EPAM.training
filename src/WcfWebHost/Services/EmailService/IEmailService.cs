using BusinessLogic.BusinessModels;

namespace WcfWebHost.Services.EmailService
{
	public interface IEmailService
	{
		void Send(object sender, OrderEventArgs args);
	}
}
