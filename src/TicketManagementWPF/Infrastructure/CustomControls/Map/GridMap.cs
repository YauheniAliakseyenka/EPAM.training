using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TicketManagementWPF.Helpers;
using TicketManagementWPF.Infrastructure.CustomControls.Map;

namespace TicketManagementWPF.Infrastructure.CustomControls
{
	internal class GridMap : Grid
	{
		private Brush SelectedCellBrush = Brushes.Brown;
		private Brush DefaultCellBrush = Brushes.Aquamarine;
		private Brush DefaultGridLineBrush = Brushes.Aqua;
		private Brush DefaultSeatMapOverviewGridLineBrush = Brushes.White;
		private Brush DefaultSelectedCellBorderBrush = Brushes.Blue;
		private int RowCount;
		private int ColumnCount;
		private double PixelColumnWidth;
		private double PixelRowHeight;
		private bool isSeatMapOverview;
		private bool isShowSolidGridLines;
		private double MinEmptyCellSize;

		#region ShowSolidGridLines Property

		public static readonly DependencyProperty ShowSolidGridLinesProperty =
			DependencyProperty.Register(
				"ShowSolidGridLines",
				typeof(bool),
				typeof(GridMap),
				new PropertyMetadata(false, ShowSolidGridLinesChanged));

		public static bool GetShowSolidGridLines(DependencyObject obj)
		{
			return (bool)obj.GetValue(ShowSolidGridLinesProperty);
		}

		public static void SetShowSolidGridLines(DependencyObject obj, bool value)
		{
			obj.SetValue(ShowSolidGridLinesProperty, value);
		}

		public static void ShowSolidGridLinesChanged(
			DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			if (!(obj is GridMap grid))
				return;

			grid.isShowSolidGridLines = (bool)grid.GetValue(ShowSolidGridLinesProperty);
		}

		#endregion

		#region SelectedCell Property

		public static readonly DependencyPropertyKey SelectedCellPropertyKey =
			DependencyProperty.RegisterReadOnly(
				"SelectedCell",
				typeof(Coordinates),
				typeof(GridMap),
				new PropertyMetadata(new Coordinates(-1, -1)));

		public static readonly DependencyProperty SelectedCellProperty = SelectedCellPropertyKey.DependencyProperty;

		#endregion

		#region SeatMapOverview Property

		public static readonly DependencyProperty SeatMapOverviewProperty =
			DependencyProperty.Register(
				"SeatMapOverview",
				typeof(bool),
				typeof(GridMap),
				new PropertyMetadata(false, SeatMapOverviewChanged));

		public static bool GetSeatMapOverview(DependencyObject obj)
		{
			return (bool)obj.GetValue(SeatMapOverviewProperty);
		}

		public static void SetSeatMapOverview(DependencyObject obj, bool value)
		{
			obj.SetValue(SeatMapOverviewProperty, value);
		}

		public static void SeatMapOverviewChanged(
			DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			if (!(obj is GridMap grid))
				return;

			grid.isSeatMapOverview = (bool)grid.GetValue(SeatMapOverviewProperty);
		}

		#endregion

		#region MinEmptyCellSize Property (if SetFlexibleGrid is true)

		private const double DefaultCellSize = 55;
		public static readonly DependencyProperty MinEmptyCellSizeProperty =
			DependencyProperty.Register(
				"MinEmptyCellSize", typeof(double), typeof(GridMap),
				new PropertyMetadata(DefaultCellSize, MinEmptyCellSizeChanged));

		public static double GetMinEmptyCellSize(DependencyObject obj)
		{
			return (double)obj.GetValue(MinEmptyCellSizeProperty);
		}

		public static void SetMinEmptyCellSize(DependencyObject obj, double value)
		{
			obj.SetValue(MinEmptyCellSizeProperty, value);
		}

		public static void MinEmptyCellSizeChanged(
			DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			if (!(obj is GridMap grid))
				return;

			grid.MinEmptyCellSize = (double)grid.GetValue(MinEmptyCellSizeProperty);
		}

		#endregion

		#region RowCount Property

		public static readonly DependencyProperty RowCountProperty =
			DependencyProperty.Register(
				"RowCount", typeof(int), typeof(GridMap),
				new PropertyMetadata(-1, RowCountChanged));

		public static int GetRowCount(DependencyObject obj)
		{
			return (int)obj.GetValue(RowCountProperty);
		}

		public static void SetRowCount(DependencyObject obj, int value)
		{
			obj.SetValue(RowCountProperty, value);
		}

		public static void RowCountChanged(
			DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			if (!(obj is GridMap) || (int)e.NewValue < 0)
				return;

			var grid = (GridMap)obj;
			grid.RowDefinitions.Clear();

			var rowHeight = (double)grid.GetValue(PixelRowHeightProperty);

			GridLength height;
			if (rowHeight > 0)
				height = new GridLength(rowHeight, GridUnitType.Pixel);
			else
				height = GridLength.Auto;

			for (int i = 0; i < (int)e.NewValue; i++)
				grid.RowDefinitions.Add(
					new RowDefinition() { Height = height });

			grid.RowCount = (int)e.NewValue;
		}

		#endregion

		#region ColumnCount Property

		public static readonly DependencyProperty ColumnCountProperty =
			DependencyProperty.Register(
				"ColumnCount", typeof(int), typeof(GridMap),
				new PropertyMetadata(-1, ColumnCountChanged));

		public static int GetColumnCount(DependencyObject obj)
		{
			return (int)obj.GetValue(ColumnCountProperty);
		}

		public static void SetColumnCount(DependencyObject obj, int value)
		{
			obj.SetValue(ColumnCountProperty, value);
		}

		public static void ColumnCountChanged(
			DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			if (!(obj is GridMap) || (int)e.NewValue < 0)
				return;

			GridMap grid = (GridMap)obj;
			grid.ColumnDefinitions.Clear();

			var columnWidth = (double)grid.GetValue(PixelRowHeightProperty);

			GridLength width;
			if (columnWidth > 0)
				width = new GridLength(columnWidth, GridUnitType.Pixel);
			else
				width = GridLength.Auto;

			for (int i = 0; i < (int)e.NewValue; i++)
				grid.ColumnDefinitions.Add(
					new ColumnDefinition() { Width = width });

			grid.ColumnCount = (int)e.NewValue;
		}

		#endregion

		#region PixelColumnWidth Property

		public static readonly DependencyProperty PixelColumnWidthProperty =
			DependencyProperty.Register(
				"PixelColumnWidth", typeof(double), typeof(GridMap),
				new PropertyMetadata(double.NaN, PixelColumnWidthChanged));

		public static double GetPixelColumnWidth(DependencyObject obj)
		{
			return (double)obj.GetValue(PixelColumnWidthProperty);
		}

		public static void SetPixelColumnWidth(DependencyObject obj, double value)
		{
			obj.SetValue(PixelColumnWidthProperty, value);
		}

		public static void PixelColumnWidthChanged(
			DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			if (!(obj is GridMap) || (double)e.NewValue < 0)
				return;

			GridMap grid = (GridMap)obj;
			foreach (var column in grid.ColumnDefinitions)
				column.Width = new GridLength((double)grid.GetValue(PixelColumnWidthProperty), GridUnitType.Pixel);

			grid.PixelColumnWidth = (double)e.NewValue;
		}

		#endregion

		#region PixelRowHeight Property

		public static readonly DependencyProperty PixelRowHeightProperty =
			DependencyProperty.Register(
				"PixelRowHeight", typeof(double), typeof(GridMap),
				new PropertyMetadata(double.NaN, PixelRowHeightChanged));

		public static double GetPixelRowHeight(DependencyObject obj)
		{
			return (double)obj.GetValue(PixelRowHeightProperty);
		}

		public static void SetPixelRowHeight(DependencyObject obj, double value)
		{
			obj.SetValue(PixelRowHeightProperty, value);
		}

		public static void PixelRowHeightChanged(
			DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			if (!(obj is GridMap) || (double)e.NewValue < 0)
				return;

			GridMap grid = (GridMap)obj;

			foreach (var row in grid.RowDefinitions)
				row.Height = new GridLength((double)grid.GetValue(PixelRowHeightProperty), GridUnitType.Pixel);

			grid.PixelRowHeight = (double)e.NewValue;
		}

		#endregion

		#region CellBrush Property

		public static readonly DependencyProperty CellBrushProperty =
			DependencyProperty.Register(
				"CellBrush", typeof(Brush), typeof(GridMap),
				new PropertyMetadata(Brushes.Aquamarine, PixelRowHeightChanged));

		public static double GetCellBrush(DependencyObject obj)
		{
			return (double)obj.GetValue(PixelRowHeightProperty);
		}

		public static void SetCellBrush(DependencyObject obj, double value)
		{
			obj.SetValue(PixelRowHeightProperty, value);
		}

		public static void CellBrushtChanged(
			DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			if (!(obj is GridMap) || (double)e.NewValue < 0)
				return;

			GridMap grid = (GridMap)obj;

			foreach (var row in grid.RowDefinitions)
				row.Height = new GridLength((double)grid.GetValue(PixelRowHeightProperty), GridUnitType.Pixel);

			grid.PixelRowHeight = (double)e.NewValue;
		}

		#endregion

		public GridMap()
        {
            this.PreviewMouseLeftButtonDown += SetSelectedCell;
            this.PreviewMouseLeftButtonDown += PreviewMouseLeftButtonClicked;
            this.PreviewMouseLeftButtonDown += DoDrag;
            this.Drop += DoDrop;
        }
		
		private void DoDrop(object sender, DragEventArgs e)
		{
			var grid = sender as GridMap;
			var point = e.GetPosition(grid);

			var coords = CalculateMouseCoordinatesOverGrid(point, grid);

			var data = e.Data.GetData(typeof(DraggedData)) as DraggedData;

			if (data is null)
				return;

			var content = data.Content as IGridMapItem;

			var dropPointContent = grid.Children.Cast<ContentPresenter>().SingleOrDefault(x =>
						GetRow(x) == coords.Row && GetColumn(x) == coords.Column);

			if (dropPointContent != null && dropPointContent.Content is IGridMapItem exchangeArea)
			{
				exchangeArea.Column = content.Column;
				exchangeArea.Row = content.Row;
			}

			content.Column = coords.Column;
			content.Row = coords.Row;
		}

		private void DoDrag(object sender, MouseButtonEventArgs e)
		{
			var grid = sender as GridMap;

			if (isSeatMapOverview)
				return;

			var selectedCell = (Coordinates)grid.GetValue(SelectedCellProperty);
			var cellContent = grid.Children.Cast<ContentPresenter>().SingleOrDefault(x =>
						GetRow(x) == selectedCell.Row && GetColumn(x) == selectedCell.Column);

			if (cellContent is null ||  isSeatMapOverview || !(cellContent.Content is IGridMapItem))
				return;
			
			var label = VisualTreeChildrenHelper.FindVisualChildren<Label>(cellContent).SingleOrDefault();
			var data = new DraggedData(cellContent.Content);

			DragDrop.DoDragDrop(label, data, DragDropEffects.All);
		}
		
		private void SetSelectedCell(object sender, MouseButtonEventArgs e)
		{
			var grid = sender as Grid;
			var point = Mouse.GetPosition(grid);

			var coords = CalculateMouseCoordinatesOverGrid(point, sender as GridMap);

			grid.SetValue(SelectedCellPropertyKey, coords);
		}

		private void PreviewMouseLeftButtonClicked(object sender, MouseButtonEventArgs e)
		{
			ProcessSelectedCell(sender);
		}

		private void ProcessSelectedCell(object sender)
		{
			if (isSeatMapOverview || !(sender is GridMap grid))
				return;

			var selectedCell = (Coordinates)grid.GetValue(SelectedCellProperty);
			var contentPresenter = grid.Children.Cast<ContentPresenter>().
				SingleOrDefault(x => GetRow(x) == selectedCell.Row && GetColumn(x) == selectedCell.Column);

			if (contentPresenter is null)
				return;
			
			var label = VisualTreeChildrenHelper.FindVisualChildren<Label>(contentPresenter).FirstOrDefault();
			label.SetValue(BackgroundProperty, SelectedCellBrush);

			grid.Children.Cast<ContentPresenter>().Where(x => !ReferenceEquals(x, contentPresenter)).ToList().ForEach(x =>
			{
				label = VisualTreeChildrenHelper.FindVisualChildren<Label>(x).FirstOrDefault();
				label.SetValue(BackgroundProperty, DefaultCellBrush);
			});
		}

		protected override void OnRender(DrawingContext dc)
		{
			ShowGridLines = false;
			base.OnRender(dc);

			if (isShowSolidGridLines)
				DrawMap(dc, DefaultGridLineBrush);
			else if (isSeatMapOverview)
				DrawMap(dc, DefaultSeatMapOverviewGridLineBrush);

			CheckVisibleItems();
		}

		private Coordinates CalculateMouseCoordinatesOverGrid(Point p, GridMap grid)
		{
			int row = 0;
			int column = 0;
			double accumulatedHeight = 0.0;
			double accumulatedWidth = 0.0;
			// calc row mouse was over
			foreach (var rowDefinition in grid.RowDefinitions)
			{
				accumulatedHeight += rowDefinition.ActualHeight;
				if (accumulatedHeight >= p.Y)
					break;
				row++;
			}

			// calc col mouse was over
			foreach (var columnDefinition in grid.ColumnDefinitions)
			{
				accumulatedWidth += columnDefinition.ActualWidth;
				if (accumulatedWidth >= p.X)
					break;
				column++;
			}

			return new Coordinates(row, column);
		}

		private class DraggedData
		{
			public object Content { get; set; }

			public DraggedData(object content)
			{
				Content = content;
			}

			public DraggedData() { }
		}

		private void CheckVisibleItems()
		{
			foreach(var child in this.Children)
			{
				if (!(child is Label label))
					return;

				var myRow = (label.Content as IGridMapItem)?.Row;
				var myCol = (label.Content as IGridMapItem)?.Column;

				if (myRow > RowCount || myCol > ColumnCount)
					label.Visibility = Visibility.Collapsed;
				else
				if (label.Visibility == Visibility.Collapsed)
					label.Visibility = Visibility.Visible;
			}
		}

		private void DrawOverviewMap(DrawingContext dc)
		{
			foreach (var row in RowDefinitions)
			{
				row.Height = new GridLength(0, GridUnitType.Star);
				row.MinHeight = 55;
			}

			foreach (var column in ColumnDefinitions)
			{
				column.Width = new GridLength(0, GridUnitType.Star);
				column.MinWidth = 55;
			}
		}

		private void DrawMap(DrawingContext dc, Brush gridBrush)
		{
			foreach (var row in RowDefinitions)
			{
				dc.DrawLine(new Pen(gridBrush, 1),
				new Point(0, row.Offset),
				new Point(ActualWidth, row.Offset));

				if (isSeatMapOverview)
				{
					row.Height = new GridLength(0, GridUnitType.Star);
					row.MinHeight = (double)GetValue(MinEmptyCellSizeProperty);
				}
			}

			foreach (var column in ColumnDefinitions)
			{
				dc.DrawLine(new Pen(gridBrush, 1),
				new Point(column.Offset, 0),
				new Point(column.Offset, ActualHeight));

				if (isSeatMapOverview)
				{
					column.Width = new GridLength(0, GridUnitType.Star);
					column.MinWidth = (double)GetValue(MinEmptyCellSizeProperty);
				}
			}
		}
	}
}
