using System.Security;
using System.Windows;

namespace TicketManagementWPF.Infrastructure.AttachedProperty
{
	internal class PasswordBoxProperties
	{
		public static SecureString GetEncryptedPassword(DependencyObject obj)
		{
			return (SecureString)obj.GetValue(EncryptedPasswordProperty);
		}

		public static void SetEncryptedPassword(DependencyObject obj, SecureString value)
		{
			obj.SetValue(EncryptedPasswordProperty, value);
		}
		
		public static readonly DependencyProperty EncryptedPasswordProperty =
			DependencyProperty.RegisterAttached("EncryptedPassword", typeof(SecureString), typeof(PasswordBoxProperties));
	}
}
