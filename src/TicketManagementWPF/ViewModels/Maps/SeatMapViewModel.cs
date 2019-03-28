using System.Linq;
using System.Threading.Tasks;
using TicketManagementWPF.Models;

namespace TicketManagementWPF.ViewModels
{
	internal class SeatMapViewModel : MapViewModelAbstract
	{
		public override Task Initialize()
		{
			DisplayView = this;
            DisplayObjectChanged += SetTempalte;

            return Task.FromResult(0);
		}

        private void SetTempalte(object obj)
        {
            SetLayoutGridTempalte(obj);
        }

        private void SetLayoutGridTempalte(object obj)
        {
            if (obj is Layout layout)
            {
                CellSize = 50;

                if (!layout.List.Any())
                    return;

                ColumnCount = layout.List.Max(x => x.Column) + 2;
                RowCount = layout.List.Max(x => x.Row) + 2;

				Title = (DisplayObject as Layout)?.Description;
			}
        }
    }
}
