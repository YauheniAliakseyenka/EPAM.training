using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TicketManagementWPF.Infrastructure;
using TicketManagementWPF.Infrastructure.Utils.Validation;
using TicketManagementWPF.Infrastructure.Validation.Utils;

namespace TicketManagementWPF.Models
{
    public class Venue: PropertyChangedBase
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

        private string _name;
		[CustomRequired("NameLabel", typeof(l10n.VenueView.View))]
		public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
			}
        }

        private string _description;
		[CustomRequired("DescriptionLabel", typeof(l10n.VenueView.View))]
		public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        private string _address;
		[CustomRequired("AddressLabel", typeof(l10n.VenueView.View))]
		public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
                OnPropertyChanged();
            }
        }

        private string _phone;
		[CustomRequired("PhoneLabel", typeof(l10n.VenueView.View))]
		public string Phone
        {
            get { return _phone; }
            set
            {
                _phone = value;
                OnPropertyChanged();
            }
        }

        private string _timezone;
		[CustomRequired("TimezoneLabel", typeof(l10n.VenueView.View))]
		public string Timezone
		{
			get { return _timezone; }
			set
			{
				_timezone = value;
				OnPropertyChanged();
			}
		}

        private ObservableCollection<Layout> _layoutList;
        public ObservableCollection<Layout> LayoutList
        {
            get { return _layoutList; }
            set
            {
                _layoutList = value;
                OnPropertyChanged();
            }
        }

		public Venue()
		{
			LayoutList = new ObservableCollection<Layout>();
			Address = string.Empty;
			Timezone = string.Empty;
			Phone = string.Empty;
			Description = string.Empty;
			Name = string.Empty;
		}

		public IEnumerable<string> Validate()
		{
			return AttributeValidator.GetOnlyErrors(this);
		}
	}
}
