using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace TicketManagementWPF.Infrastructure.Behaviors
{
    internal class ConfirmBoxBehavior : Behavior<UIElement>
	{
        #region MessageBoxText Property

        public static readonly DependencyProperty MessageBoxTextProperty =
            DependencyProperty.Register(
                "MessageBoxText", typeof(string), typeof(ConfirmBoxBehavior),
                new PropertyMetadata(string.Empty));

        public static string GetMessageBoxText(DependencyObject obj)
        {
            return (string)obj.GetValue(MessageBoxTextProperty);
        }

        public static void SetMessageBoxText(DependencyObject obj, string value)
        {
            obj.SetValue(MessageBoxTextProperty, value);
        }

        #endregion

        #region TitleText Property

        public static readonly DependencyProperty TitleTextProperty =
            DependencyProperty.Register(
                "TitleText", typeof(string), typeof(ConfirmBoxBehavior),
                new PropertyMetadata(string.Empty));

        public static string GetTitleText(DependencyObject obj)
        {
            return (string)obj.GetValue(TitleTextProperty);
        }

        public static void SetTitleText(DependencyObject obj, string value)
        {
            obj.SetValue(TitleTextProperty, value);
        }

        #endregion

        protected override void OnAttached()
		{
			AssociatedObject.PreviewMouseLeftButtonDown += Clicked;
		}
		
		protected override void OnDetaching()
		{
			AssociatedObject.PreviewMouseLeftButtonDown -= Clicked;
		}

		private void Clicked(object sender, MouseButtonEventArgs e)
		{
            string messageBoxText = (string)this.GetValue(MessageBoxTextProperty);
            string title = (string)this.GetValue(TitleTextProperty);
            MessageBoxButton button = MessageBoxButton.YesNo;
			MessageBoxImage icon = MessageBoxImage.Warning;
			
			var result = MessageBox.Show(messageBoxText, title, button, icon);
			switch (result)
			{
				case MessageBoxResult.Yes:
					e.Handled = false;
					break;
				case MessageBoxResult.No:
					e.Handled = true;
					break;
			}
		}
	}
}
