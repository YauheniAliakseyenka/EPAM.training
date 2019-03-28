using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace TicketManagementWPF.Infrastructure.Extensions.Localization
{
    internal class TranslateSource : INotifyPropertyChanged
    {
        public static TranslateSource Instance { get; } = new TranslateSource();

        public event PropertyChangedEventHandler PropertyChanged;

        public string this[string key, string resourcePath]
        {
            get
            {
				if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(resourcePath))
					return string.Empty;

				if (_resourceManager is null || !_resourceManager.BaseName.Equals(resourcePath, StringComparison.Ordinal))
                    _resourceManager = new ResourceManager(resourcePath, Assembly.GetExecutingAssembly());

                return _resourceManager.GetString(key, this._culture);
            }
        }

        private CultureInfo _culture = CultureInfo.CurrentCulture;
        public CultureInfo CurrentCulture
        {
            get { return this._culture; }
            set
            {
                if (this._culture != value)
                {
                    this._culture = value;
					CultureInfo.CurrentCulture = value;
					CultureInfo.CurrentUICulture = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
                }
            }
        }

        private ResourceManager _resourceManager;
    }
}
