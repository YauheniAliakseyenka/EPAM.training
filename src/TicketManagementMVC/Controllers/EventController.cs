using BusinessLogic.DTO;
using BusinessLogic.Exceptions.EventExceptions;
using BusinessLogic.Services;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TicketManagementMVC.Infrastructure;
using TicketManagementMVC.Infrastructure.Authentication;
using TicketManagementMVC.Models.Event;

namespace TicketManagementMVC.Controllers
{
    [CustomAuthorize(Roles = "User")]
    public class EventController : Controller
    {
        private IAuthenticationManager _authManager => HttpContext.GetOwinContext().Authentication;
        private UserManager<User, string> _userManager;
        private IStoreService<VenueDto, int> _venueService;
        private IStoreService<LayoutDto, int> _layoutService;
        private IEventService _eventService;
		private IStoreService<EventAreaDto, int> _eventAreaService;
		private IStoreService<EventSeatDto, int> _eventSeatService;

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

        public EventController(IStoreService<VenueDto, int> venueService,
            UserManager<User, string> userManager,
			IStoreService<LayoutDto, int> layoutService,
            IEventService eventService,
			IStoreService<EventAreaDto, int> eventAreaService,
			IStoreService<EventSeatDto, int> eventSeatService)
        {
            _venueService = venueService;
            _userManager = userManager;
            _layoutService = layoutService;
            _eventService = eventService;
			_eventAreaService = eventAreaService;
			_eventSeatService = eventSeatService;
        }

        [HttpGet]
        public ActionResult Create()
        {
			ViewBag.VenueList = _venueService.GetList();
			return View(new EventViewModel());
        }

        [HttpPost]
        public ActionResult Create(EventViewModel model, HttpPostedFileBase image)
        {
            if (!ModelState.IsValid)
            {
				return Json(new
				{
					success = false,
					errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
								  .Select(m => m.ErrorMessage).ToArray()
				});
			}

            try
            {
                string url;
                string path;
				url = getImageUrl(image, out path);
				var s = model.Date.Date + model.Time.TimeOfDay;
				_eventService.Create(new EventDto
				{
					Date = model.Date.Date + model.Time.TimeOfDay,
					Description = model.Description,
					ImageURL = url,
					LayoutId = model.LayoutId,
					Title = model.Title,
					CreatedBy = User.Identity.GetUserId()
				});

				if (!string.IsNullOrEmpty(path))
					image.SaveAs(path);
            }
            catch (EventException exception)
            {
                string error = string.Empty;

                if (exception.Message.Equals("Invalid date", StringComparison.OrdinalIgnoreCase))
                    error = I18N.ResourceErrors.EventInvalidDateError;

                if (exception.Message.Equals("Attempt of creating event with a date in the past", StringComparison.OrdinalIgnoreCase))
                    error = I18N.ResourceErrors.PastDateError;

                ModelState.AddModelError(string.Empty, error);
            }

            if (ModelState.Values.Where(x => x.Errors.Any()).Any())
            {
				return Json(new
				{
					success = false,
					errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
								   .Select(m => m.ErrorMessage).ToArray()
				});
			}

			return Json(new
			{
				success = true
			});
		}

        [HttpGet]
		[NoDirectAccess]
		public JsonResult GetLayouts(int venueId)
        {
            return Json(new {
                layouts = _layoutService.FindBy(x => x.VenueId == venueId).Select(x => new { Id = x.Id, Display = x.Description }).ToList()
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
		[NoDirectAccess]
		public JsonResult GetEventsByVenue(int venueId)
        {
			return Json(new
			{
				events = _eventService.GetEventsByVenueIdForParticularEventManager(venueId, User.Identity.GetUserId()).Select(x => new
				{
					Id = x.Id,
					Display = x.Title + " (" + x.Date.ToShortDateString() + ", " + x.Date.ToShortTimeString() + ")"
				}).ToList()
			}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
		public ActionResult EditArea(int areaId, int eventId)
        {
			var area = _eventService.GetEventStructure(eventId).Areas.Where(x => x.Id == areaId).FirstOrDefault();

			if (area == null)
				throw new NullReferenceException();

			var model = new EventAreaViewModel
			{
				SeatList = area.Seats,
				CoordX = area.CoordX,
				CoordY = area.CoordY,
				Description = area.Description,
				Id = area.Id,
				Price = area.Price
			};

			ViewBag.Title = I18N.Resource.EditAreaTitle;
			ViewBag.Action = "EditArea";
			return PartialView("~/Views/Event/Partial/EventArea.cshtml", model);
        }

		[HttpPost]
		public ActionResult EditArea(EventAreaViewModel model)
		{
			if (model.SeatList == null)
			{
				return Json(new
				{
					success = false,
					errors = new string[] { I18N.ResourceErrors.SeatListError }
				});
			}

			if (!ModelState.IsValid)
				return Json(new
				{
					success = false,
					errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
								 .Select(m => m.ErrorMessage).ToArray()
				});
			
            try
            {
				var area = _eventAreaService.Get(model.Id);
				area.CoordX = model.CoordX;
				area.CoordY = model.CoordY;
				area.Description = model.Description;
				area.Price = model.Price;
                area.Seats = model.SeatList;
				_eventAreaService.Update(area);
			}
			catch (EventSeatException exception)
			{
				string error = string.Empty;

				if (exception.Message.Equals("Seat already exists", StringComparison.OrdinalIgnoreCase))
					error = I18N.ResourceErrors.SeatExistsError;

				return Json(new
				{
					success = false,
					errors = new string[] { error }
				});
			}
			catch (EventAreaException exception)
			{
				string error = string.Empty;

				if (exception.Message.Equals("Area description isn't unique", StringComparison.OrdinalIgnoreCase))
					error = I18N.ResourceErrors.AreaDescriptionError;

				return Json(new
				{
					success = false,
					errors = new string[] { error }
				});
			}

            return Json(new
			{
				success = true
			});
		}

        [HttpPost]
        public ActionResult CreateArea(EventAreaViewModel model)
        {
			if (model.SeatList == null)
				return Json(new
				{
					success = false,
					errors = new string[] { I18N.ResourceErrors.SeatListError }
				});

			if (!ModelState.IsValid)
                return Json(new
                {
                    success = false,
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                                 .Select(m => m.ErrorMessage).ToArray()
                });
			
            try
            {
                var area = new EventAreaDto
                {
                    CoordX = model.CoordX,
                    CoordY = model.CoordY,
                    Description = model.Description,
                    Price = model.Price,
                    Seats = model.SeatList,
                    EventId = model.EventId
                };
                _eventAreaService.Create(area);
            }
			catch(EventSeatException exception)
			{
				string error = string.Empty;

				if (exception.Message.Equals("Seat already exists", StringComparison.OrdinalIgnoreCase))
					error = I18N.ResourceErrors.SeatExistsError;

				return Json(new
				{
					success = false,
					errors = new string[] { error }
				});
			}
            catch (EventAreaException exception)
            {
                string error = string.Empty;

                if (exception.Message.Equals("Area description isn't unique", StringComparison.OrdinalIgnoreCase))
                    error = I18N.ResourceErrors.AreaDescriptionError;

				return Json(new
				{
					success = false,
					errors = new string[] { error }
				});
			}

            return Json(new
            {
                success = true
            });
        }

        [HttpGet]
        public ActionResult CreateArea(int eventId)
        {
			ViewBag.Title = I18N.Resource.CreateAreaTitle;
			ViewBag.Action = "CreateArea";
			return PartialView("~/Views/Event/Partial/EventArea.cshtml", new EventAreaViewModel
            {
                SeatList = new List<EventSeatDto>(),
                EventId = eventId
            });
        }

        [HttpGet]
        public ActionResult Edit()
        {
            ViewBag.VenueList = _venueService.GetList();
            return View();
        }

		[HttpPost]
		public ActionResult Edit(EditEventViewModel model, HttpPostedFileBase image)
		{
			if (!ModelState.IsValid)
			{
				return Json(new
				{
					success = false,
					errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
								  .Select(m => m.ErrorMessage).ToArray()
				});
			}

			try
			{
				string newImagePath = string.Empty;
				string newImageUrl = string.Empty;
				string oldImageUrl = string.Empty;
				
				var update = _eventService.Get(model.Event.Id);

				if (image != null && image.ContentLength > 0)
				{
					newImageUrl = getImageUrl(image, out newImagePath);
					oldImageUrl = update.ImageURL;
				}

				update.Date = model.Event.Date.Date + model.Event.Time.TimeOfDay;
				update.Description = model.Event.Description;
				update.ImageURL = string.IsNullOrEmpty(newImageUrl) ? update.ImageURL : newImageUrl;
				update.LayoutId = model.Event.LayoutId;
				update.Title = model.Event.Title;
				_eventService.Update(update);

				if (!string.IsNullOrEmpty(newImageUrl))
				{
					image.SaveAs(newImagePath);

					//delete old image
					string fileName = Path.GetFileName(oldImageUrl);
					if (!fileName.Equals("default", StringComparison.OrdinalIgnoreCase))
					{
						string fullPath = Server.MapPath("~/Content/images/uploads/" + fileName);
						System.IO.File.Delete(fullPath);
					}
				}
			}
			catch (EventException exception)
			{
				string error = string.Empty;

				if (exception.Message.Equals("Invalid date", StringComparison.OrdinalIgnoreCase))
					error = I18N.ResourceErrors.EventInvalidDateError;

				if (exception.Message.Equals("Attempt of creating event with a date in the past", StringComparison.OrdinalIgnoreCase))
					error = I18N.ResourceErrors.PastDateError;

				if (exception.Message.Equals("Not allowed to update layout. Event has locked seats", StringComparison.OrdinalIgnoreCase))
					error = I18N.ResourceErrors.EventLayoutChangeError;

				return Json(new
				{
					success = false,
					errors = new string[] { error }
				});
			}

			return Json(new
			{
				success = true
			});
		}

		[HttpGet]
		[NoDirectAccess]
		public ActionResult GetEventToEdit(int eventId)
        {
            var data = _eventService.GetEventStructure(eventId);

			var edit = new EventViewModel
			{
				Date = data.Event.Date.Date,
				Time = data.Event.Date,
				Description = data.Event.Description,
				Title = data.Event.Title,
				LayoutId = data.Event.LayoutId,
				ImageURL = data.Event.ImageURL,
				Id = data.Event.Id
			};

			ViewBag.VenueList = _venueService.GetList();
			return PartialView("~/Views/Event/Partial/GetEventToEdit.cshtml", new EditEventViewModel
            {
                Event = edit,
                EventAreas = data.Areas.ToList()
            });
        }

		[NoDirectAccess]
		public ActionResult DeleteArea(int areaId)
		{
			try
			{
				_eventAreaService.Delete(areaId);
				return Json(new
				{
					success = true
				});
			}
			catch(DbUpdateException)
			{
				return Json(new
				{
					success = false,
					error = I18N.ResourceErrors.SeatLockedError
				});
			}
		}

		[NoDirectAccess]
		public ActionResult DeleteEvent(int eventId)
		{
			try
			{
				_eventService.Delete(eventId);
			}
			catch (EventException exception)
			{
				string error = string.Empty;

				if (exception.Message.Equals("Not allowed to delete. Event has locked seat", StringComparison.OrdinalIgnoreCase))
					error = I18N.ResourceErrors.DeleteEventError;

				return Json(new
				{
					success = false,
					error
				});
			}

			return Json(new
			{
				success = true
			});
		}

		private string getImageUrl(HttpPostedFileBase image, out string path)
		{
			string url;
			if (image != null && image.ContentLength > 0)
			{
				var fileName = Path.GetFileName(image.FileName);
				path = Path.Combine(Server.MapPath("~/Content/images/uploads"), fileName);
				url = new Uri(Request.Url, Url.Content("~/Content/images/uploads/" + image.FileName)).ToString();
				return url;
			}
			else
			{
				url = new Uri(Request.Url, Url.Content("~/Content/Images/default.jpg")).ToString();
				path = string.Empty;
				return url;
			}
		}
    }
}