using System;
using System.Linq;
using System.Windows.Data;
using TicketManagementWPF.Models;

namespace TicketManagementWPF.Infrastructure.Convertors
{
	internal class GridMaxRowConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var item = value as Area;

			var maxRow = item.List.Max(x => x.Row);

			return maxRow + 2;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
