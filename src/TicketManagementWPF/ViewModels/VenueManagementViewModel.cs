using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TicketManagementWPF.Helpers.Parsers;
using TicketManagementWPF.Helpers.WindowManagement;
using TicketManagementWPF.Infrastructure;
using TicketManagementWPF.Infrastructure.Commands;
using TicketManagementWPF.VenueService;

namespace TicketManagementWPF.ViewModels
{
    public class VenueManagementViewModel: ViewModelAbstract
	{
		public const string VenueSavedOperationKey = "VenueSaved";

		#region Commands

		public ICommand AddVenueCommand { get; private set; }
		public ICommand EditVenueCommand { get; private set; }
		public ICommand DeleteVenueCommand { get; private set; }

		#endregion

		#region Binding properties

		private ObservableCollection<Models.Venue> _venueList;
        public ObservableCollection<Models.Venue> VenueList
        {
            get { return _venueList; }
			set
			{
				_venueList = value;
				OnPropertyChanged();
			}
        }

		private List<string> _errors;
		public List<string> Errors
		{
			get { return _errors; }
			set
			{
				_errors = value;
				OnPropertyChanged();
			}
		}

		#endregion

		private Models.Venue _markedVenue;

		private readonly IWindowHelper _windowHelper;
		private readonly IMediator _mediator;
		private readonly IWcfVenueService _venueService;

        public VenueManagementViewModel(
            IWindowHelper windowHelper, 
            IMediator mediator, 
            IWcfVenueService venueService)
        {
            _windowHelper = windowHelper;
			_mediator = mediator;
			_venueService = venueService;
		}

        private async Task OnAddVenue(object obj)
        {
			await OpenVenueWindow(new Models.Venue(), string.Format(l10n.VenueView.View.CreateTitle, string.Empty));
        }

		private async Task OnEditVenue(object obj)
		{
			if (!(obj is Models.Venue venue))
				return;

			var edit = await _venueService.GetFullModelAsync(venue.Id);
			venue.LayoutList = VenueParser.ToVenue(edit).LayoutList;

			_markedVenue = venue;
			await OpenVenueWindow(venue, string.Format(l10n.VenueView.View.EditTitle, edit.Name));
			_markedVenue = null;
		}

		private async Task OnDeleteVenue(object obj)
		{
			DisplayError(string.Empty);

			if (!(obj is Models.Venue venue))
				return;

			try
			{
				await _venueService.DeleteAsync(venue.Id);
			}
            catch (FaultException<ServiceValidationFaultDetails> exception)
            {
                if (exception.Message.Equals("Not allowed to delete. Venue has events setted up", StringComparison.OrdinalIgnoreCase))
                    DisplayError(l10n.VenueView.Errors.VenueDelete);

                return;
            }
            catch (FaultException)
			{
                DisplayError(l10n.Shared.Errors.InternalAppError);
				return;
			}

			VenueList.Remove(venue);
		}

		private async Task OpenVenueWindow(Models.Venue venue, string title)
		{
			DisplayError(string.Empty);
			var viewModel = await _windowHelper.GetViewModel<VenueViewModel>() as VenueViewModel;
			viewModel.Venue = venue;
			viewModel.Title = title;
			_windowHelper.ShowDialog(viewModel);
		}

		public override async Task Initialize()
		{
			AddVenueCommand = new RelayCommandAsync(OnAddVenue);
			EditVenueCommand = new RelayCommandAsync(OnEditVenue);
			DeleteVenueCommand = new RelayCommandAsync(OnDeleteVenue);
			_mediator.Subscribe(VenueSavedOperationKey, VenueSaved);

			var venues = await _venueService.ToListAsync();
			VenueList = new ObservableCollection<Models.Venue>(venues.Select(x => VenueParser.ToVenue(x)).ToList());
		}

		private void VenueSaved(object obj)
		{
			DisplayError(null);
			if (!(obj is Models.Venue venue))
				return;

			if (_markedVenue is null)
			{
				VenueList.Add(venue);
				return;
			}

			var index = VenueList.IndexOf(_markedVenue);
			VenueList.Remove(_markedVenue);
			VenueList.Insert(index, venue);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposed)
				return;

			if (disposing)
			{
				_mediator.Unsubscribe(VenueSavedOperationKey, VenueSaved);
			}

			disposed = true;
		}

        private void DisplayError(string error)
        {
            if (Errors is null)
                Errors = new List<string>();
            else
                Errors.Clear();

            Errors.Add(error);
            OnPropertyChanged(nameof(Errors));
        }
    }
}
