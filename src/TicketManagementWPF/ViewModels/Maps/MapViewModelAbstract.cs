using System.Collections.Generic;
using System.Windows.Input;
using TicketManagementWPF.Helpers.WindowManagement;
using TicketManagementWPF.Infrastructure;
using TicketManagementWPF.Infrastructure.Commands;
using TicketManagementWPF.Infrastructure.Extensions;

namespace TicketManagementWPF.ViewModels
{
	public abstract class MapViewModelAbstract : ViewModelAbstract, IDisplayWindow
	{
		protected delegate void DispalyObjectHandler(object obj);
		protected event DispalyObjectHandler DisplayObjectChanged;

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
			get
			{
				return _title;
			}
			set
			{
				_title = value;
				
				OnPropertyChanged();
			}
		}

		private object _displayObject;
		public object DisplayObject
		{
			get { return _displayObject; }
			set
			{
				var cloned = value.DeepClone();
				_displayObject = cloned;
				DisplayObjectChanged?.Invoke(cloned);
				OnPropertyChanged();
			}
		}

		public double CellSize { get; set; }
		public int RowCount { get; set; }
		public int ColumnCount { get; set; }

		public List<string> Hints { get; protected set; }

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

        public ICommand ClickCellCommand { get; protected set; }
		public ICommand SaveCommand { get; protected set; }
		public ICommand ApplySettingsCommand { get; protected set; }

		#endregion

		public MapViewModelAbstract()
		{
			ApplySettingsCommand = new RelayCommand(OnApplySettings);
		}

		public bool IsChanged { get; protected set; }

		protected virtual void OnApplySettings(object obj)
		{
			OnPropertyChanged(nameof(CellSize));
			OnPropertyChanged(nameof(RowCount));
			OnPropertyChanged(nameof(ColumnCount));
		}
	}
}
