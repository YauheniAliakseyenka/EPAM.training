using System.Windows.Controls;
using System.Windows.Interactivity;
using TicketManagementWPF.Infrastructure.AttachedProperty;

namespace TicketManagementWPF.Infrastructure.Behaviors
{
	internal class PasswordBoxBehavior : Behavior<PasswordBox>
	{
		protected override void OnAttached()
		{
			AssociatedObject.PasswordChanged += PasswordChanged;
		}
		
		protected override void OnDetaching()
		{
			AssociatedObject.PasswordChanged -= PasswordChanged;
		}

		private void PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
		{
			var pswdbox = sender as PasswordBox;
			pswdbox.SetValue(PasswordBoxProperties.EncryptedPasswordProperty, pswdbox.SecurePassword);
		}
	}
}
