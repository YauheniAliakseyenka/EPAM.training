using System.Collections.Generic;
using TicketManagementWPF.Infrastructure;
using TicketManagementWPF.Infrastructure.CustomControls.Map;
using TicketManagementWPF.Infrastructure.Validation.Utils;

namespace TicketManagementWPF.Models
{
	public class Seat : PropertyChangedBase, IGridMapItem
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

        private int _areaId;
        public int AreaId
        {
            get { return _areaId; }
            set
            {
                _areaId = value;
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
		

		public IEnumerable<string> Validate()
        {
            return AttributeValidator.GetOnlyErrors(this);
        }
    }
}
