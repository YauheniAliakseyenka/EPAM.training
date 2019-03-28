using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace TicketManagementWPF.Helpers
{
	internal static class VisualTreeChildrenHelper
	{
		public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
		{
			if (depObj != null)
			{
				for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
				{
					DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
					if (child != null && child is T)
					{
						yield return (T)child;
					}

					foreach (T childOfChild in FindVisualChildren<T>(child))
					{
						yield return childOfChild;
					}
				}
			}
		}

		public static T FindParent<T>(DependencyObject child) where T : DependencyObject
		{
			DependencyObject parentObject = VisualTreeHelper.GetParent(child);
			
			if (parentObject == null) return null;
			
			T parent = parentObject as T;
			if (parent != null)
				return parent;
			else
				return FindParent<T>(parentObject);
		}
	}
}
