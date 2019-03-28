using System;
using System.Text;
using System.Windows.Data;
using TicketManagementWPF.Models;

namespace TicketManagementWPF.Infrastructure.Convertors
{
    internal class GetTextFromNodeConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
			var sb = new StringBuilder();

			if (value is Layout layout)
			{
				sb.Append(layout.Description).Append(l10n.Map.View.AreaCount).Append(layout.List.Count);
			}

			if (value is Area area)
			{
				sb.Append(area.Description).Append(l10n.Map.View.SeatCount).Append(area.List.Count);
			}

            return sb.ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
