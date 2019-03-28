using System.Collections.Generic;
using System.Collections.ObjectModel;
using TicketManagementWPF.Infrastructure;
using TicketManagementWPF.Infrastructure.Utils.Validation;
using TicketManagementWPF.Infrastructure.Validation.Utils;

namespace TicketManagementWPF.Models
{
	public class Layout : PropertyChangedBase
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

        private int _venueId;
        public int VenueId
        {
            get { return _venueId; }
            set
            {
                _venueId = value;
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


        private ObservableCollection<Area> _areaList;
        public ObservableCollection<Area> List
        {
            get { return _areaList; }
            set
            {
                _areaList = value;
                OnPropertyChanged();
            }
        }

		public Layout()
		{
			List = new ObservableCollection<Area>();
			Description = string.Empty;
		}

        public IEnumerable<string> Validate()
        {
            return AttributeValidator.GetOnlyErrors(this);
        }
    }
}
