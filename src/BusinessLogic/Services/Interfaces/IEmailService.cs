using BusinessLogic.BusinessModels;

namespace BusinessLogic.Services
{
	public interface IEmailService
	{
		void Send(object sender, OrderEventArgs args);
	}
}
