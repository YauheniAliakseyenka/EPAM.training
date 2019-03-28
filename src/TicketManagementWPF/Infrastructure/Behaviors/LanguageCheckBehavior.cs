using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using TicketManagementWPF.Infrastructure.Extensions.Localization;

namespace TicketManagementWPF.Infrastructure.Behaviors
{
    internal class LanguageCheckBehavior : Behavior<MenuItem>
    {
        protected override void OnAttached()
        {
            AssociatedObject.Click += Click;
            AssociatedObject.Loaded += Loaded;
        } 

        protected override void OnDetaching()
        {
            AssociatedObject.Click -= Click;
            AssociatedObject.Loaded -= Loaded;
        }

        private void Click(object sender, RoutedEventArgs e)
        {
            var menuItem = (MenuItem)e.OriginalSource;

            menuItem.IsChecked = true;
			TranslateSource.Instance.CurrentCulture = new CultureInfo(Convert.ToString(menuItem.Tag));

            GetCheckableSubMenuItems(AssociatedObject)
                .Where(item => !ReferenceEquals(item, menuItem))
                .ToList()
                .ForEach(item => item.IsChecked = false);
        }

        private void Loaded(object sender, RoutedEventArgs e)
        {
            var culture = TranslateSource.Instance.CurrentCulture.TwoLetterISOLanguageName;
			GetCheckableSubMenuItems(AssociatedObject).
				SingleOrDefault(x =>
				{
					var tag = Convert.ToString(x.Tag);
					return tag.Equals(culture, StringComparison.OrdinalIgnoreCase);
				}).IsChecked = true;
		}

        private static IEnumerable<MenuItem> GetCheckableSubMenuItems(ItemsControl menuItem)
        {
            var itemCollection = menuItem.Items;
            return itemCollection.OfType<MenuItem>().Where(x => x.IsCheckable);
        }
    }
}
