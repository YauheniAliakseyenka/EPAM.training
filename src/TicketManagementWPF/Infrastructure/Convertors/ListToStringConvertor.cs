using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace TicketManagementWPF.Infrastructure.Convertors
{
	internal class ListToStringConvertor : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is IEnumerable<string> errors))
				return value;

            if (!errors.Any())
                return null;

			var sb = new StringBuilder();

			foreach (var error in errors)
				sb.Append(error).Append(Environment.NewLine);

			return sb.ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
