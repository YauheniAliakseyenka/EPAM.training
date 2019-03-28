using System;
using System.Globalization;
using System.Windows.Data;

namespace TicketManagementWPF.Infrastructure.Convertors
{
	internal class TextBoxLengthConvertor : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is string str))
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
