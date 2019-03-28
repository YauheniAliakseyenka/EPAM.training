using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using TicketManagementWPF.Infrastructure.CustomControls;
using TicketManagementWPF.ViewModels;

namespace TicketManagementWPF.Infrastructure.Behaviors
{
	internal class DeleteAreaFromMapBehavior : Behavior<GridMap>
	{
		protected override void OnAttached()
		{
			AssociatedObject.PreviewMouseRightButtonDown += RightMousePressed;
		}
		
		protected override void OnDetaching()
		{
			AssociatedObject.PreviewMouseRightButtonDown -= RightMousePressed;
		}

		private void RightMousePressed(object sender, MouseButtonEventArgs e)
		{
			var window = Window.GetWindow(sender as DependencyObject);

			if (!(window.DataContext is LayoutMapViewModel layoutViewModel))
				return;

			string messageBoxText = l10n.Map.View.DeleteAreaMsgBoxText;
			string caption = l10n.Map.View.DeleteAreaMsgBoxTitle;
			MessageBoxButton button = MessageBoxButton.YesNo;
			MessageBoxImage icon = MessageBoxImage.Warning;

			if(layoutViewModel.HasSelectedArea)
			{
				var result = MessageBox.Show(messageBoxText, caption, button, icon);

				if (result == MessageBoxResult.Yes)
					layoutViewModel.DeleteSelectedAreaCommand?.Execute(null);
			}
		}
	}
}
