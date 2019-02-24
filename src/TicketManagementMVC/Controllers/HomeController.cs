using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using TicketManagementMVC.Helpers;
using TicketManagementMVC.Infrastructure.Authentication;
using TicketManagementMVC.EventService;
using System.Threading;

namespace TicketManagementMVC.Controllers
{
	public class HomeController : Controller
    {
		private IWcfEventService _eventService;
		private AuthManager _authManager => new AuthManager(this.HttpContext);

		private static int _eventPageSize;

		protected override void Initialize(RequestContext requestContext)
		{
			base.Initialize(requestContext);
			var identity = HttpContext.User.Identity;
			if (identity.IsAuthenticated)
				ViewData["Balance"] = _authManager.GetAccountBalance();
				
		}

		public HomeController(IWcfEventService eventService)
		{
			_eventService = eventService;
			_eventPageSize = 10;
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<ActionResult> Index(string filterText, int index = 0, 
			FilterEventOptions filterBy = FilterEventOptions.None, bool isFirstLoad = true)
		{
			var data = await _eventService.GetPublishedEventsAsync(filterBy, filterText, Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);
			ViewBag.Count = data.Count();
			ViewBag.PageSize = _eventPageSize;
			ViewBag.CurrentIndex = index;
			ViewBag.FilterOption = filterBy;
			ViewBag.FilterText = filterText;

			if (isFirstLoad)
				return View(data.Skip(index).Take(_eventPageSize));
			else
				return PartialView("~/Views/Home/Partial/_EventList.cshtml", data.Skip(index).Take(_eventPageSize));
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<ActionResult> EventInfo(int id)
		{
			var displayEvent = await _eventService.GetEventInformationAsync(id);

			if (displayEvent == null || !displayEvent.IsPublished)
				return RedirectToAction("Index");

			return View(displayEvent);
		}

        [AllowAnonymous]
        public ActionResult SetCulture(string culture)
        {
            CultureSetter.Set(culture, this);

            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}