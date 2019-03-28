using System.ComponentModel;
using System.Windows;
using System.Windows.Interactivity;
using TicketManagementWPF.ViewModels;

namespace TicketManagementWPF.Infrastructure.Behaviors
{
    internal class SaveMapBehavior : Behavior<Window>
	{
		protected override void OnAttached()
		{
			AssociatedObject.Closing += OnClosing;
		}

		protected override void OnDetaching()
		{
			AssociatedObject.Closing -= OnClosing;
		}

		private void OnClosing(object sender, CancelEventArgs e)
		{
			var window = sender as Window;
			if (!(window.DataContext is MapViewModelAbstract context) || !context.IsChanged)
				return;

			string messageBoxText = l10n.Shared.SharedResources.SaveQuestionMsgBoxText;
			string caption = l10n.Shared.SharedResources.SaveQuestionMsgBoxTitle;
			MessageBoxButton button = MessageBoxButton.YesNo;
			MessageBoxImage icon = MessageBoxImage.Question;

			var result = MessageBox.Show(messageBoxText, caption, button, icon);
			
			if(result == MessageBoxResult.Yes)
				context.SaveCommand.Execute(context.DisplayObject);

			e.Cancel = false;
		}
	}
}
