using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using TicketManagementWPF.Infrastructure.AttachedProperty;

namespace TicketManagementWPF.Infrastructure.Behaviors
{
	internal class DynamicFontSizeBehavior : Behavior<Control>
	{
		protected override void OnAttached()
		{
			AssociatedObject.SizeChanged += SizeChanged;
		}
		
		protected override void OnDetaching()
		{
			AssociatedObject.SizeChanged -= SizeChanged;
		}

		private void SizeChanged(object sender, SizeChangedEventArgs e)
		{
			var window = Window.GetWindow(AssociatedObject);

			var tempScale = (double)AssociatedObject.GetValue(CommonProperties.FontScaleProperty);
			var scale = tempScale == default(double) ? 0.1 : tempScale;

			var Height = window.ActualHeight * scale;
			var Width = window.ActualWidth * scale;

			AssociatedObject.FontSize = Math.Min(Height, Width);
		}
	}
}
