using BusinessLogic.Services;
using BusinessLogic.Services.EventServices;
using Microsoft.AspNet.Identity;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using TicketManagementMVC.Helpers;
using TicketManagementMVC.Infrastructure.Attributes;
using TicketManagementMVC.Infrastructure.Authentication;

namespace TicketManagementMVC.Controllers
{
	public class HomeController : Controller
    {
		private ApplicationUserManager _userManager;
		private IEventService _eventService;
        private ICartService _cartService;

		protected override async void Initialize(RequestContext requestContext)
		{
			base.Initialize(requestContext);
			var identity = HttpContext.User.Identity;
			if (identity.IsAuthenticated)
			{
				var user = await _userManager.FindByNameAsync(identity.GetUserName());
				ViewData["Balance"] = DisplayBalance.Get(user.Amount);
			}
		}

		public HomeController(IEventService eventService, ICartService cartService, ApplicationUserManager userManager)
		{
			_eventService = eventService;
            _cartService = cartService;
			_userManager = userManager;
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<ActionResult> Index()
        {
            return View(await _eventService.GetPublishedEvents(FilterEventOptions.None));
        }

		[NoDirectAccess]
		public async Task<ActionResult> FilterList(string filterText, FilterEventOptions filterBy = FilterEventOptions.None)
		{
			return PartialView("~/Views/Home/Partial/EventList.cshtml", await _eventService.GetPublishedEvents(filterBy, filterText));
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
    }
}