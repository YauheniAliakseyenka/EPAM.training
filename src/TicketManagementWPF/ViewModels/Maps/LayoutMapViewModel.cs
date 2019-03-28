using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TicketManagementWPF.Helpers.WindowManagement;
using TicketManagementWPF.Infrastructure;
using TicketManagementWPF.Infrastructure.Commands;
using TicketManagementWPF.Models;

namespace TicketManagementWPF.ViewModels
{
    internal class LayoutMapViewModel : MapViewModelAbstract
	{
		public const string SaveAreaOperationKey = "SaveArea";
		
		public bool HasSelectedArea { get; private set; } 
		private Area _selectedArea;

		#region Commands

		public ICommand DeleteSelectedAreaCommand { get; private set; }
		public ICommand EditAreaCommand { get; private set; }

		#endregion

		private readonly IMediator _mediator;
		private readonly IWindowHelper _windowHelper;

		public LayoutMapViewModel(IMediator mediator, IWindowHelper windowHelper)
		{
			_mediator = mediator;
			_windowHelper = windowHelper;
		}

		private void SetDefaultTempalte(object obj)
		{
			if (obj is Layout layout)
			{
				CellSize = 50;

				if (layout.List.Any())
				{
					ColumnCount = layout.List.Max(x => x.Column) + 2;
					RowCount = layout.List.Max(x => x.Row) + 2;
				}
				else
				{
					ColumnCount = 5;
					RowCount = 5;
				}

				IsChanged = false;
				
				Title = (DisplayObject as Layout)?.Description;
			}
		}

		private void OnSave(object obj)
		{
			if (DisplayObject is Layout layout)
			{
				Errors = layout.Validate().ToList();

				if (Errors.Any())
					return;

				_mediator.Raise(VenueViewModel.AcceptChangesOperationKey, DisplayObject);

				IsChanged = false;
			}
		}

		private async Task OnClick(object obj)
		{
			if (obj is null)
				throw new ArgumentNullException("Cell coordinates expected as parameter");

			if (DisplayObject is null)
				return;
		
			var coords = (Coordinates)obj;
			var layout = DisplayObject as Layout;
			var area = layout.List.SingleOrDefault(x => x.Row == coords.Row && x.Column == coords.Column);

			if (area is null)
			{
				area = new Area
				{
					Column = coords.Column,
					Row = coords.Row
				};

                var viewmodel = await _windowHelper.GetViewModel<AreaMapViewModel>() as AreaMapViewModel;
                viewmodel.DisplayObject = area;
                _windowHelper.ShowDialog(viewmodel);
				SetSelectedArea(area, true);

				return;
			}

			SetSelectedArea(area, true);
		}

		private async Task OnDoubleClick(object obj)
		{
			if (_selectedArea == null)
				return;

			var viewmodel = await _windowHelper.GetViewModel<AreaMapViewModel>() as AreaMapViewModel;
			viewmodel.DisplayObject = _selectedArea;
			_windowHelper.ShowDialog(viewmodel);
		}

		private void ChangeArea(object obj)
		{
			var layout = DisplayObject as Layout;
			var area = obj as Area;
			var currentArea = layout?.List.SingleOrDefault(x => x.Row == area.Row && x.Column == area.Column);

			SetSelectedArea(area, true);

			if(currentArea != null)
				layout.List.Remove(currentArea);
			
			layout.List.Add(area);
			IsChanged = true;
		}	

		private void DeleteArea(object obj)
		{
			var layout = DisplayObject as Layout;

			if (layout is null || _selectedArea is null)
				return;

			layout.List.Remove(_selectedArea);

			SetSelectedArea(null, false);
		}

		private void SetSelectedArea(Area area , bool flag)
		{
			_selectedArea = area;
			HasSelectedArea = flag;
		}

		public override Task Initialize()
		{
			ClickCellCommand = new RelayCommandAsync(OnClick);
			SaveCommand = new RelayCommand(OnSave);
			DeleteSelectedAreaCommand = new RelayCommand(DeleteArea);
			EditAreaCommand = new RelayCommandAsync(OnDoubleClick);
			DisplayView = this;

			_mediator.Subscribe(SaveAreaOperationKey, ChangeArea);
			DisplayObjectChanged += SetDefaultTempalte;

			Hints = new List<string>
			{
				l10n.Map.Hints.AddArea,
				l10n.Map.Hints.DeleteArea,
				l10n.Map.Hints.EditArea,
				l10n.Map.Hints.ItemMove
			};

			return Task.FromResult(0);
		}

        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                _mediator.Unsubscribe(SaveAreaOperationKey, ChangeArea);
            }

            disposed = true;
        }
    }
}
