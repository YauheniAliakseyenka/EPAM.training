using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TicketManagementWPF.Helpers.Parsers;
using TicketManagementWPF.Helpers.WindowManagement;
using TicketManagementWPF.Infrastructure;
using TicketManagementWPF.Infrastructure.Commands;
using TicketManagementWPF.Models;
using TicketManagementWPF.VenueService;
using System.ServiceModel;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace TicketManagementWPF.ViewModels
{
    public class VenueViewModel : ViewModelAbstract, IDisplayWindow
    {
		public const string AcceptChangesOperationKey = "AcceptVenueChanges";
		public const string ClearMarkOnEditLayoutOperationKey = "CancelMarkOnEditLayout";

		#region Binding properties

		private object _displayView;
		public object DisplayView
		{
			get
			{
				return _displayView;
			}
			set
			{
				_displayView = value;
				OnPropertyChanged();
			}
		}

		private string _title;
		public string Title
		{
			get { return _title; }
			set
			{
				_title = value;
				OnPropertyChanged();
			}
		}
	
		private Models.Venue _venue;
		public Models.Venue Venue
		{
			get { return _venue; }
			set
			{
				_venue = value;
				OnPropertyChanged();

				Venue.PropertyChanged -= NameChanged;
				Venue.PropertyChanged += NameChanged;
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

        #region Commands

        public ICommand AddLayoutCommand { get; private set; }
		public ICommand ShowSeatMapCommand { get; private set; }
		public ICommand DeleteLayoutCommand { get; private set; }
		public ICommand EditLayoutCommand { get; private set; }
		public ICommand SaveCommand { get; private set; }
		public ICommand OnClosed { get; private set; }

        #endregion

        private Models.Layout _markedLayoutToEdit;
		private readonly IWindowHelper _windowHelper;
		private readonly IMediator _mediator;
		private readonly IWcfVenueService _venueService;

		public VenueViewModel(
			IWcfVenueService venueService, 
			IWindowHelper windowHelper, 
			IMediator mediator)
        {
			_windowHelper = windowHelper;
			_mediator = mediator;
            _venueService = venueService;
        }

		private async Task OnSave(object obj)
		{
            if (Venue is null)
                return;

            Errors = Venue.Validate().ToList();

            if (Errors.Any())
                return;

			var addOrUpdate = VenueParser.ToVenueContract(Venue);

			try
			{
				if (addOrUpdate.Id <= 0)
					Venue.Id = await _venueService.CreateAsync(addOrUpdate);
				else
				{
					await _venueService.UpdateAsync(addOrUpdate);
					Venue.Id = addOrUpdate.Id;
				}

				//update ids of areas, which were update (for large count of seats, venue service deletes areas and inserts it in order to increase performance)
				var venue = _venueService.GetFullModel(Venue.Id);
				Venue = VenueParser.ToVenue(venue);

				_mediator.Raise(VenueManagementViewModel.VenueSavedOperationKey, Venue);
			}
			catch (FaultException<ServiceValidationFaultDetails> exception)
			{
				switch (exception.Message)
				{
					case "Such venue already exists":
						DisplayError(l10n.VenueView.Errors.VenueNameIsExists);
						break;
					case "Incorrect state of the venue. The venue must have at least one layout":
						DisplayError(l10n.VenueView.Errors.VenueHasNoLayouts);
						break;
					case "Area description isn't unique":
						DisplayError(l10n.VenueView.Errors.AreaDescriptionUnique);
						break;
					case "Incorrect state of area. An area must have atleast one seat":
						DisplayError(l10n.VenueView.Errors.AreaHasNoSeats);
						break;
					case "Layout description isn't unique":
						DisplayError(l10n.VenueView.Errors.LayoutDescriptionUnique);
						break;
					case "Incorrect state of the layout. The layout must have at least one area":
						DisplayError(l10n.VenueView.Errors.LayoutDescriptionUnique);
						break;
				}
			}
			catch (FaultException)
			{
				DisplayError(l10n.Shared.Errors.InternalAppError);
			}
		}

		/// <summary>
		/// Open new window to add new layout
		/// </summary>
		/// <param name="obj"></param>
        private async void OnAddLayout(object obj)
        {
			var vm = await _windowHelper.GetViewModel<LayoutMapViewModel>() as LayoutMapViewModel;
			vm.DisplayObject = new Models.Layout();
			_windowHelper.ShowDialog(vm);
		}

		/// <summary>
		/// Delete layout
		/// </summary>
		/// <param name="obj">Layout</param>
		private Task OnDeleteLayout(object obj)
		{
			if (!(obj is Models.Layout layout))
				return Task.FromResult(0);

			return Task.FromResult(Venue.LayoutList.Remove(layout));
		}

		// <summary>
		/// Open new window to edit selected layout
		/// </summary>
		/// <param name="obj">Layout</param>
		private async void OnEditLayout(object obj)
		{
			if (!(obj is Models.Layout layout))
				return;

			var vm = await _windowHelper.GetViewModel<LayoutMapViewModel>() as LayoutMapViewModel;
			vm.DisplayObject = layout;
			_markedLayoutToEdit = layout;
			_windowHelper.ShowDialog(vm);
		}

		/// <summary>
		/// Callback method to insert a layout to list
		/// </summary>
		/// <param name="obj">Layout</param>
		private void AcceptChanges(object obj)
		{
			if (!(obj is Models.Layout layout))
				return;

			int index = Venue.LayoutList.Count;
			if (_markedLayoutToEdit != null)
			{
				index = Venue.LayoutList.IndexOf(_markedLayoutToEdit);
				Venue.LayoutList.Remove(_markedLayoutToEdit);
				_markedLayoutToEdit = layout;
			}

			Venue.LayoutList.Insert(index, layout);
		}

		/// <summary>
		/// Display whole seat map of layout to view (only)
		/// </summary>
		/// <param name="obj">Layout</param>
		public async Task OnShowSeatMap(object obj)
		{
			if (!(obj is Models.Layout layout))
				return;
			
			var viewmodel = await _windowHelper.GetViewModel<SeatMapViewModel>() as SeatMapViewModel;
			viewmodel.DisplayObject = layout;
			_windowHelper.ShowDialog(viewmodel);
		}

		private void ClearMarkedLayoutToEdit(object obj = null)
		{
			_markedLayoutToEdit = null;
		}

		public override Task Initialize()
		{
			DisplayView = this;

			_mediator.Subscribe(AcceptChangesOperationKey, AcceptChanges);
			_mediator.Subscribe(ClearMarkOnEditLayoutOperationKey, ClearMarkedLayoutToEdit);

			AddLayoutCommand = new RelayCommand(OnAddLayout);
			ShowSeatMapCommand = new RelayCommandAsync(OnShowSeatMap);
			DeleteLayoutCommand = new RelayCommandAsync(OnDeleteLayout);
			EditLayoutCommand = new RelayCommand(OnEditLayout);
			SaveCommand = new RelayCommandAsync(OnSave);

			return Task.FromResult(0);
		}

		private void NameChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName.Equals(nameof(Venue.Name), StringComparison.Ordinal))
				Title = Regex.Replace(
					Title,
					@"(?<=[:]\s*).*",
				 " " + (sender as Models.Venue).Name);
				
		}

		protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                _mediator.Unsubscribe(AcceptChangesOperationKey, AcceptChanges);
                _mediator.Unsubscribe(ClearMarkOnEditLayoutOperationKey, ClearMarkedLayoutToEdit);
				PropertyChanged -= NameChanged;
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
