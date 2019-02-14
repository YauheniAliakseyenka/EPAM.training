using AutomatedTests.WebPages;
using AutomatedTests.WebPages.Hooks;
using System;

namespace AutomatedTests.Utils
{
    public class PageFactory
    {
		private static readonly Lazy<PageFactory> Lazy = new Lazy<PageFactory>(() => new PageFactory());

		public static PageFactory Instance => Lazy.Value;

        public T GetPage<T>() where T: AbstractWebPage
        {
			object[] args = { GlobalHooks.Driver };

			return (T)Activator.CreateInstance(typeof(T), args);
        }
    }
}
