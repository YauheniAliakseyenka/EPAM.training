using System;
using System.Collections.Generic;
using System.Windows.Markup;
using TicketManagementWPF.Models;

namespace TicketManagementWPF.Infrastructure.Extensions
{
    public class TimezoneCollectionExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var timezones = new List<TimezoneItemModel>();
            foreach (var timezone in TimeZoneInfo.GetSystemTimeZones())
            {
                var item = new TimezoneItemModel
                {
                    Id = timezone.Id,
                    DisplayText = timezone.DisplayName
                };

                timezones.Add(item);
            }

            return timezones;
        }
    }
}
