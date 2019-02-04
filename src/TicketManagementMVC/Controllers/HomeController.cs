using BusinessLogic.Services;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using TicketManagementMVC.Helpers;
using TicketManagementMVC.Infrastructure.Authentication;

namespace TicketManagementMVC.Controllers
{
	public class HomeController : Controller
    {
		private ApplicationUserManager _userManager;
		private IEventService _eventService;

		private static int _eventPageSize;

		protected override async void Initialize(RequestContext requestContext)
		{
			base.Initialize(requestContext);
			var identity = HttpContext.User.Identity;
			if (identity.IsAuthenticated)
			{
				var user = await _userManager.FindByNameAsync(identity.GetUserName());
				ViewData["Balance"] = user.Amount;
			}
		}

		public HomeController(IEventService eventService, ApplicationUserManager userManager)
		{
			_eventService = eventService;
			_userManager = userManager;
			_eventPageSize = 10;
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<ActionResult> Index(string filterText, int index = 0, FilterEventOptions filterBy = FilterEventOptions.None, bool isFirstLoad = true)
		{
			var data = await _eventService.GetPublishedEvents(filterBy, filterText);
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
			var displayEvent = await _eventService.GetEventInformation(id);

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