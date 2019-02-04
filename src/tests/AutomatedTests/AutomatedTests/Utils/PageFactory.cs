using AutomatedTests.WebPages;
using AutomatedTests.WebPages.Hooks;
using OpenQA.Selenium;
using System;
using TechTalk.SpecFlow;

namespace AutomatedTests.Utils
{
    public class PageFactory
    {
		private static readonly Lazy<PageFactory> Lazy = new Lazy<PageFactory>(() => new PageFactory());

		public static PageFactory Instance => Lazy.Value;

        public T GetPage<T>() where T: AbstractWebPage
        {
			object[] args = { (IWebDriver)ScenarioContext.Current[GlobalHooks.DriverKey] };

			return (T)Activator.CreateInstance(typeof(T), args);
        }
    }
}
