using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Markup;
using TicketManagementWPF.Helpers;

namespace TicketManagementWPF.Infrastructure.Extensions
{
	internal class CultureMenuItemEnumerationExtension : MarkupExtension
	{
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			var enumValues = Enum.GetValues(typeof(Culture));

			return (
			  from Enum enumValue in enumValues
			  select new MenuItem
			  {
				  Header = EnumHelper.GetDescription(enumValue),
				  Tag = enumValue,
				  IsCheckable = true
			  }).ToArray();
		}
	}
}
