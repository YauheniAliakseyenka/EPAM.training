namespace TicketManagementWPF.Helpers.WindowManagement
{
	public interface IDisplayWindow
	{
		object DisplayView { get; }
		string Title { get;  }
	}
}
