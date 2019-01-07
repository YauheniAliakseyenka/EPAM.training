using BusinessLogic.DTO;
using BusinessLogic.Exceptions;
using BusinessLogic.Services;
using Hangfire;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TicketManagementMVC.Infrastructure;
using TicketManagementMVC.Infrastructure.Authentication;

namespace TicketManagementMVC.Controllers
{
	public class HomeController : Controller
    {
		private IAuthenticationManager _authManager => HttpContext.GetOwinContext().Authentication;
		private UserManager<User, string> _userManager;
		private IEventService _eventService;
        private ICartService _cartService;

		protected override void Initialize(RequestContext requestContext)
		{
			base.Initialize(requestContext);
			var identity = HttpContext.User.Identity;
			if (identity.IsAuthenticated)
			{
				var user = _userManager.FindByName(identity.GetUserName());
				Thread.CurrentThread.CurrentCulture = new CultureInfo(user.Culture);
				Thread.CurrentThread.CurrentUICulture = new CultureInfo(user.Culture);
				ViewData["Balance"] = string.Format("${0:N2}", user.Amount);
			}
		}

		public HomeController(IEventService eventService, ICartService cartService, UserManager<User, string> userManager)
		{
			_eventService = eventService;
            _cartService = cartService;
			_userManager = userManager;
		}

		[HttpGet]
		[AllowAnonymous]
		public  ActionResult Index()
        {
			return View(_eventService.GetPublishedEvents());
        }

		[HttpGet]
		[AllowAnonymous]
		public ActionResult EventInfo(int id)
		{
			var displayEvent = _eventService.GetEventStructure(id);

			if (displayEvent == null || !displayEvent.IsPublished)
				return RedirectToAction("Index");

			return View(displayEvent);
		}

        [NoDirectAccess]
        public ActionResult AddSeatToCart(int seatId)
        {
			try
			{
				int lockPeriod;
				_cartService.AddSeat(seatId, User.Identity.GetUserId());
				
				if (!int.TryParse(ConfigurationManager.AppSettings["SeatLockTime"], out lockPeriod))
					throw new Exception("Seat lock period is not valid");

				RecurringJob.AddOrUpdate("unlockSeatId" + seatId, () => unlockSeat(seatId), Cron.MinuteInterval(lockPeriod));
			}
			catch(CartException exception)
			{
				string error = string.Empty;

				if (exception.Message.Equals("Seat is locked", StringComparison.OrdinalIgnoreCase))
					error = I18N.ResourceErrors.SeatLocked;
				else
					throw;

				return Json(new
				{
					success = true,
					error = error
				});
			}

			
			return Json(new
			{
				success = true
			});
		}

		[NoDirectAccess]
		public void unlockSeat(int seatId)
		{
			_cartService.UnlockSeat(seatId);
			RecurringJob.RemoveIfExists("unlockSeatId" + seatId);
		}
    }
}