using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagementWPF.Helpers.WindowManagement;
using TicketManagementWPF.Infrastructure;
using TicketManagementWPF.Infrastructure.Commands;
using TicketManagementWPF.Models;

namespace TicketManagementWPF.ViewModels
{
	internal class AreaMapViewModel : MapViewModelAbstract
	{
		private readonly IMediator _mediator;
		private readonly IWindowHelper _windowHelper;

		public AreaMapViewModel(IMediator mediator, IWindowHelper windowHelper)
		{
			_mediator = mediator;
			_windowHelper = windowHelper;
		}
		
		private void SetGridTempalte(object obj)
		{
			if (obj is Area area)
			{
				CellSize = 35;
				if(area.List.Any())
				{
					ColumnCount = area.List.Max(x => x.Column) + 2;
					RowCount = area.List.Max(x => x.Row) + 2;
				}
				else
				{
					ColumnCount = 15;
					RowCount = 15;
				}

				IsChanged = false;
				Title = (DisplayObject as Area)?.Description;
			}
		}

		private void OnSave(object obj)
		{
			if (DisplayObject is Area area)
			{
				Errors = area.Validate().ToList();

				if (Errors.Any())
					return;

				_mediator.Raise(LayoutMapViewModel.SaveAreaOperationKey, DisplayObject);

				IsChanged = false;
			}
		}

		private void OnClick(object obj)
		{
			var area = DisplayObject as Area;
			var coords = (Coordinates)obj;

			var seat = area.List.SingleOrDefault(x => x.Row == coords.Row && x.Column == coords.Column);

			if (seat is null)
			{
				seat = new Seat
				{
					Column = coords.Column,
					Row = coords.Row
				};
				area.List.Add(seat);
				IsChanged = true;
				return;
			}

			area.List.Remove(seat);
			IsChanged = true;
		}

		public override Task Initialize()
		{
			DisplayView = this;
			ClickCellCommand = new RelayCommand(OnClick);
			SaveCommand = new RelayCommand(OnSave);
			DisplayObjectChanged += SetGridTempalte;
			Hints = new List<string>
			{
				l10n.Map.Hints.AddSeat,
				l10n.Map.Hints.DeleteSeat,
				l10n.Map.Hints.ItemMove
			};
			IsChanged = false;
			return Task.FromResult(0);
		}
	}
}
