using System;
using System.Linq;
using System.Windows.Markup;
using TicketManagementWPF.Helpers;

namespace TicketManagementWPF.Infrastructure.Extensions
{
	public class EnumerationExtension : MarkupExtension
	{
		private Type _enumType;

		public EnumerationExtension(Type enumType)
		{
			EnumType = enumType ?? throw new ArgumentNullException("enumType");
		}

		public Type EnumType
		{
			get { return _enumType; }
			private set
			{
				if (_enumType == value)
					return;

				var enumType = Nullable.GetUnderlyingType(value) ?? value;

				if (enumType.IsEnum == false)
					throw new ArgumentException("Type must be an Enum.");

				_enumType = value;
			}
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			var enumValues = Enum.GetValues(EnumType);
		
			return (
			  from Enum enumValue in enumValues
			  select new EnumerationMember
			  {
				  Value = enumValue,
				  Description = EnumHelper.GetDescription(enumValue)
			  }).ToArray();
		}

        private class EnumerationMember
        {
            public string Description { get; set; }
            public object Value { get; set; }
        }
    }
}
