using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TicketManagementWPF.Infrastructure.Extensions
{
    public static partial class SecureStringExtention
    {
        public static string GetStringValue(this SecureString str)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(str);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }
    }
}
