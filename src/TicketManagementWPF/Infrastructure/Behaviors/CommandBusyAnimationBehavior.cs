using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using TicketManagementWPF.Helpers;
using TicketManagementWPF.Infrastructure.Commands;
using TicketManagementWPF.Infrastructure.Extensions.Localization;

namespace TicketManagementWPF.Infrastructure.Behaviors
{
	internal class CommandBusyAnimationBehavior : Behavior<UIElement>
	{
		#region AnimationText Property

		public static readonly DependencyProperty AnimationTextProperty =
			DependencyProperty.Register(
				"AnimationText", typeof(string), typeof(CommandBusyAnimationBehavior),
				new PropertyMetadata(l10n.Shared.SharedResources.BusyText));

		public static string GetAnimationText(DependencyObject obj)
		{
			return (string)obj.GetValue(AnimationTextProperty);
		}

		public static void SetAnimationText(DependencyObject obj, string value)
		{
			obj.SetValue(AnimationTextProperty, value);
		}

		#endregion

		protected override void OnAttached()
		{
			AssociatedObject.PreviewMouseLeftButtonDown += Click;
		}

		protected override void OnDetaching()
		{
			AssociatedObject.PreviewMouseLeftButtonDown -= Click;
		}

		private async void Click(object sender, MouseButtonEventArgs e)
		{
			if (!(AssociatedObject is ICommandSource element) || !(element.Command is RelayCommandAsync))
				return;

			e.Handled = true;
			var window = Window.GetWindow(AssociatedObject);

			var animation = VisualTreeChildrenHelper.FindVisualChildren<Border>(window).
				SingleOrDefault(x => x.Name.Equals("PulseBox", StringComparison.OrdinalIgnoreCase));
			var text = (string)this.GetValue(AnimationTextProperty);
			VisualTreeChildrenHelper.FindVisualChildren<TextBlock>(animation).SingleOrDefault().Text =
				string.IsNullOrEmpty(text) ?
				(string)this.GetValue(AnimationTextProperty) :
				TranslateSource.Instance[nameof(l10n.Shared.SharedResources.BusyText), l10n.Shared.SharedResources.ResourceManager.BaseName];

			ShowAnimation(animation);

			try
			{
				await (element.Command as RelayCommandAsync).ExecuteAsync(element.CommandParameter);
			}
			catch(Exception)
			{
				HideAnimation(animation);
				throw;
			}

			HideAnimation(animation);
		}

		private void ShowAnimation(Border animation)
		{
			animation.Visibility = Visibility.Visible;
			animation.Opacity = 0.9;
			animation.SetValue(Panel.ZIndexProperty, 2);
		}

		private void HideAnimation(Border animation)
		{
			animation.Visibility = Visibility.Collapsed;
			animation.Opacity = 0;
			animation.SetValue(Panel.ZIndexProperty, 0);
		}
	}
}
