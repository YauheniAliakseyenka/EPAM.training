using System;
using System.Linq;
using System.Windows.Data;
using TicketManagementWPF.Models;

namespace TicketManagementWPF.Infrastructure.Convertors
{
	internal class GridMaxColumnConvertor : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var item = value as Area;

			var maxColumn = item.List.Max(x => x.Column);

			return maxColumn + 2;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
