using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace TicketManagementWPF.Infrastructure.Behaviors
{
	internal class RoleListViewBehavior : Behavior<ListView>
	{
		protected override void OnAttached()
		{
			AssociatedObject.PreviewMouseDown += DoDrag;
			AssociatedObject.Drop += DoDrop;
		}

		protected override void OnDetaching()
		{
			AssociatedObject.PreviewMouseDown -= DoDrag;
			AssociatedObject.Drop -= DoDrop;
		}

		private void DoDrag(object sender, MouseButtonEventArgs e)
		{
			var dropedList = sender as ListView;

			DragDrop.DoDragDrop((ListView)e.Source, (ListView)e.Source, DragDropEffects.All);
		}

		private void DoDrop(object sender, DragEventArgs e)
		{
			var dropedList = sender as ListView;

			var dropSourceCollection = dropedList.ItemsSource as Collection<string>;

			var draggedList = e.Data.GetData(typeof(ListView)) as ListView;
			
			var draggedSourceCollection = draggedList.ItemsSource as Collection<string>;

			var str = (string)draggedList.GetValue(ListView.SelectedItemProperty);

			if (dropSourceCollection.Any(x => x.Equals(str, StringComparison.OrdinalIgnoreCase)) || string.IsNullOrEmpty(str)
				|| str.Equals("user", StringComparison.OrdinalIgnoreCase))
				return;

			draggedSourceCollection.Remove(str);
			dropSourceCollection.Add(str);
		}
	}
}
