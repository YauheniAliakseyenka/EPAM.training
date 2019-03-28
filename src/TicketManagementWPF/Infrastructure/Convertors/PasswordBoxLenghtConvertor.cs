using System;
using System.Globalization;
using System.Security;
using System.Windows.Data;

namespace TicketManagementWPF.Infrastructure.Convertors
{
	internal class PasswordBoxLengthConvertor : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is SecureString str))
				return value;

			if (str.Length > 0)
				return true;

			return false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
