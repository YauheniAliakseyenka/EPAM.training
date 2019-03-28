using System.Collections.Generic;
using System.Collections.ObjectModel;
using TicketManagementWPF.Infrastructure;
using TicketManagementWPF.Infrastructure.CustomControls.Map;
using TicketManagementWPF.Infrastructure.Utils.Validation;
using TicketManagementWPF.Infrastructure.Validation.Utils;

namespace TicketManagementWPF.Models
{
	public class Area : PropertyChangedBase, IGridMapItem
	{
        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        private int _layoutId;
        public int LayoutId
        {
            get { return _layoutId; }
            set
            {
                _layoutId = value;
                OnPropertyChanged();
            }
        }

        private int _column;
        public int Column
        {
            get { return _column; }
            set
            {
                _column = value;
                OnPropertyChanged();
            }
        }

        private int _row;
        public int Row
        {
            get { return _row; }
            set
            {
                _row = value;
                OnPropertyChanged();
            }
        }

        private string _description;
		[CustomRequired("DescriptionLabel", typeof(l10n.Map.View))]
		public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Seat> _seatList;
        public ObservableCollection<Seat> List
        {
            get { return _seatList; }
            set
            {
                _seatList = value;
                OnPropertyChanged();
            }
        }

		public Area()
		{
			List = new ObservableCollection<Seat>();
			Description = string.Empty;
		}

        public IEnumerable<string> Validate()
        {
            return AttributeValidator.GetOnlyErrors(this);
        }
    }
}
