using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TicketManagementWPF.Infrastructure
{
	public class PropertyChangedBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged([CallerMemberName]string property = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
		}
	}
}
