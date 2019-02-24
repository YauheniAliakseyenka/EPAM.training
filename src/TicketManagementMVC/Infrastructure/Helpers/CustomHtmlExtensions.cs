using System;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web.Mvc;

namespace TicketManagementMVC.Helpers
{
	public static class CustomHtmlExtensions
	{
		public static string BalanceDisplay(this HtmlHelper helper, decimal amount)
		{
			return string.Format("${0:N2}", amount);
		}

		public static string OffsetDisplay(this HtmlHelper helper, string timezoneId)
		{
            var timezone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
            var builder = new StringBuilder("(UTC");
            builder.Append(timezone.BaseUtcOffset < TimeSpan.Zero ? "-" : "+").Append(timezone.BaseUtcOffset.ToString("hh\\:mm"))
                .Append(")");

            return builder.ToString();
        }

        public static string GetEnumDescription(this HtmlHelper helper, Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }
            return en.ToString();
        }
    }
}