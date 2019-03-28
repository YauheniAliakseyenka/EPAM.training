using System.Windows.Data;

namespace TicketManagementWPF.Infrastructure.Extensions.Localization
{
    internal class Localization : Binding
    {
        public Localization(string name, string resourceFilePath) :
            base("[" + name + "," + resourceFilePath + "]")
        {
            this.Source = TranslateSource.Instance;
        }
    }
}
