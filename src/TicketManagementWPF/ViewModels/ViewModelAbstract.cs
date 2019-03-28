using System;
using System.Threading.Tasks;

namespace TicketManagementWPF.Infrastructure
{
	public abstract class ViewModelAbstract : PropertyChangedBase, IDisposable
    {
        protected bool disposed = false;

		public abstract Task Initialize();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
            }

            disposed = true;
        }

        ~ViewModelAbstract()
        {
            Dispose(false);
        }
    }
}
