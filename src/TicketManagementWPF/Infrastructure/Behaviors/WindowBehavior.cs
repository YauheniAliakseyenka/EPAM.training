using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Interactivity;

namespace TicketManagementWPF.Infrastructure.Behaviors
{
	internal class WindowBehavior : Behavior<Window>
	{
		protected override void OnAttached()
		{
			AssociatedObject.Closing += OnClosing;
			AssociatedObject.Closed += OnClosed;
			AssociatedObject.Loaded += Window_Loaded;
			AssociatedObject.SizeChanged += Window_SizeChanged;
			AssociatedObject.LocationChanged += Window_LocationChanged;
		}

		protected override void OnDetaching()
		{
			AssociatedObject.Closing -= OnClosing;
			AssociatedObject.Closed -= OnClosed;
			AssociatedObject.Loaded -= Window_Loaded;
			AssociatedObject.SizeChanged -= Window_SizeChanged;
			AssociatedObject.LocationChanged -= Window_LocationChanged;
		}

		private void Window_LocationChanged(object sender, EventArgs e)
		{
			CheckBounds();
		}

		private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			CheckBounds();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			AssociatedObject.MinWidth = SystemParameters.VirtualScreenWidth * 0.25;
			AssociatedObject.MaxWidth = SystemParameters.VirtualScreenWidth;
			AssociatedObject.MinHeight = SystemParameters.VirtualScreenHeight * 0.25;
			AssociatedObject.MaxHeight = SystemParameters.VirtualScreenHeight;
		}

		private void OnClosing(object sender, CancelEventArgs e)
		{
			if (ReferenceEquals(sender, Application.Current.MainWindow))
			{
				string messageBoxText = l10n.Shared.SharedResources.CloseAppQuestion;
				string caption = l10n.Shared.SharedResources.CloseAppQuestionTitle;
				MessageBoxButton button = MessageBoxButton.YesNo;
				MessageBoxImage icon = MessageBoxImage.Warning;

				var result = MessageBox.Show(messageBoxText, caption, button, icon);

				switch (result)
				{
					case MessageBoxResult.Yes:
						e.Cancel = false;
						break;
					case MessageBoxResult.No:
						e.Cancel = true;
						break;
				}
			}
		}

		private void OnClosed(object sender, EventArgs e)
		{
            if (sender is Window window)
                if (window.DataContext is ViewModelAbstract context)
                    context.Dispose();
		}

		private void CheckBounds()
		{
			var height = SystemParameters.PrimaryScreenHeight;
			var width = SystemParameters.PrimaryScreenWidth;

			if (AssociatedObject.Left < 0)
				AssociatedObject.Left = 0;
			if (AssociatedObject.Top < 0)
				AssociatedObject.Top = 0;
			if (AssociatedObject.Top + AssociatedObject.Height > height)
				AssociatedObject.Top = height - AssociatedObject.Height;
			if (AssociatedObject.Left + AssociatedObject.Width > width)
				AssociatedObject.Left = width - AssociatedObject.Width;
		}
	}
}
