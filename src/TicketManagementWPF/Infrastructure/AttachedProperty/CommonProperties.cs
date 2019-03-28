using System.Windows;

namespace TicketManagementWPF.Infrastructure.AttachedProperty
{
	internal class CommonProperties
	{
		#region FontScale Property

		public static readonly DependencyProperty FontScaleProperty =
			DependencyProperty.RegisterAttached(
				"ColumnCount", typeof(double), typeof(CommonProperties),
				new PropertyMetadata(default(double)));

		public static double GetFontScale(DependencyObject obj)
		{
			return (double)obj.GetValue(FontScaleProperty);
		}

		public static void SetFontScale(DependencyObject obj, double value)
		{
			obj.SetValue(FontScaleProperty, value);
		}

		#endregion
	}
}
