using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TicketManagementMVC.EventService;
using TicketManagementMVC.Infrastructure.Attributes;
using TicketManagementMVC.Infrastructure.Authentication;
using TicketManagementMVC.Models.Event;
using TicketManagementMVC.EventAreaService;
using System.ServiceModel;
using TicketManagementMVC.LayoutService;
using TicketManagementMVC.VenueService;
using WcfBusinessLogic.Core.Contracts.Exceptions;
using WcfBusinessLogic.Core.Contracts.Data;

namespace TicketManagementMVC.Controllers
{
    [Authorize(Roles = "Event manager")]
    public class EventController : Controller
    {
        private IWcfVenueService _venueService;
        private IWcfLayoutService _layoutService;
        private IWcfEventService _eventService;
		private IWcfEventAreaService _eventAreaService;
		private AuthManager _authManager => new AuthManager(this.HttpContext);

		protected override void Initialize(RequestContext requestContext)
		{
			base.Initialize(requestContext);
			var identity = HttpContext.User.Identity;
			if (identity.IsAuthenticated)
				ViewData["Balance"] = _authManager.GetAccountBalance();
		}

        public EventController(IWcfVenueService venueService,
			IWcfLayoutService layoutService,
			IWcfEventService eventService,
			IWcfEventAreaService eventAreaService)
        {
            _venueService = venueService;
            _layoutService = layoutService;
            _eventService = eventService;
			_eventAreaService = eventAreaService;
        }

        //GET: create new event view
        [HttpGet]
        public async Task<ActionResult> Create()
        {
			ViewBag.VenueList = await _venueService.ToListAsync();
			return View(new EventViewModel());
        }

		//POST: create new event
        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(EventViewModel model, HttpPostedFileBase image)
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
				url = GetImageUrl(image, out string path);

				var create = new Event
				{
					CreatedBy = User.Identity.GetUserId<int>(),
					Title = model.Title,
					LayoutId = model.LayoutId,
					ImageURL = url,
					Date = model.Date.Date + model.Time.TimeOfDay,
					Description = model.Description
				};
				await _eventService.CreateAsync(create);

				if (!string.IsNullOrEmpty(path))
					image.SaveAs(path);
            }
            catch (FaultException<ServiceValidationFaultDetails> exception)
            {
				string error = exception.Message;

                if (exception.Message.Equals("Invalid date", StringComparison.OrdinalIgnoreCase))
                    error = ProjectResources.ResourceErrors.EventInvalidDateError;

                if (exception.Message.Equals("Attempt of creating event with a date in the past", StringComparison.OrdinalIgnoreCase))
                    error = ProjectResources.ResourceErrors.PastDateError;

				return Json(new
				{
					success = false,
					errors = new string[] { error }
				});
			}
			catch (FaultException exception)
			{
				return Json(new
				{
					success = false,
					error = exception.Message
				});
			}

			return Json(new
			{
				success = true
			});
		}

        //get layouts by venue id
		[NoDirectAccess]
		public async Task<ActionResult> GetLayouts(int venueId)
        {
			var list = await _layoutService.GetLayoutsByVenueAsync(venueId);

			return Json(new {
                layouts = list.Select(x => new { x.Id, Display = x.Description }).ToList()
            }, JsonRequestBehavior.AllowGet);
        }

        //get events by venue id
        [NoDirectAccess]
		public async Task<ActionResult> GetEventsByVenue(int venueId)
        {
			var list = await _eventService.GetEventManagerEventsAsync(venueId, User.Identity.GetUserId<int>());

			return Json(new
			{
				events = list.Select(x => new
				{
					x.Id,
					Display = x.Title + " (" + x.Date.ToShortDateString() + ", " + x.Date.ToShortTimeString() + ")"
				}).ToList()
			}, JsonRequestBehavior.AllowGet);
        }

		//GET: edit area view
		[HttpGet]
		public async Task<ActionResult> EditArea(int areaId, int eventId)
		{
			var data = await _eventService.GetEventInformationAsync(eventId);
			var area = data.Areas.Where(x => x.Id == areaId).FirstOrDefault();

			var model = new EventAreaViewModel
			{
				SeatList = area.Seats.Select(x => new EventSeat
				{
					State = x.State,
					EventAreaId = x.EventAreaId,
					Id = x.Id,
					Number = x.Number,
					Row = x.Row
				}).ToList(),
				CoordX = area.CoordX,
				CoordY = area.CoordY,
				Description = area.Description,
				Id = area.Id,
				Price = area.Price
			};

			ViewBag.Title = ProjectResources.EventResource.Edit;
			ViewBag.Action = "EditArea";
			return PartialView("~/Views/Event/Partial/_EventArea.cshtml", model);
		}

		//POST: edit area
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> EditArea(EventAreaViewModel model)
		{
			if (model.SeatList == null)
			{
				return Json(new
				{
					success = false,
					errors = new string[] { ProjectResources.ResourceErrors.SeatListError }
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
				var area = await _eventAreaService.GetAsync(model.Id);
				area.CoordX = model.CoordX;
				area.CoordY = model.CoordY;
				area.Description = model.Description;
				area.Price = model.Price;
				area.Seats = model.SeatList.ToArray();
				await _eventAreaService.UpdateAsync(area);
			}
			catch (FaultException<ServiceValidationFaultDetails> exception)
			{
				string error = string.Empty;

				if (exception.Message.Equals("Seat already exists", StringComparison.OrdinalIgnoreCase))
					error = ProjectResources.ResourceErrors.SeatExistsError;

				if (exception.Message.Equals("Area description isn't unique", StringComparison.OrdinalIgnoreCase))
					error = ProjectResources.ResourceErrors.AreaDescriptionError;

				return Json(new
				{
					success = false,
					errors = new string[] { error }
				});
			}
			catch (FaultException exception)
			{
				return Json(new
				{
					success = false,
					error = exception.Message
				});
			}

			return Json(new
			{
				success = true
			});
		}

        //POST: create new area
        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> CreateArea(EventAreaViewModel model)
        {
			if (model.SeatList == null)
			{
				return Json(new
				{
					success = false,
					errors = new string[] { ProjectResources.ResourceErrors.SeatListError }
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
                var area = new EventArea
                {
                    CoordX = model.CoordX,
                    CoordY = model.CoordY,
                    Description = model.Description,
                    Price = model.Price,
                    Seats = model.SeatList.ToArray(),
                    EventId = model.EventId
                };
                await _eventAreaService.CreateAsync(area);
            }
			catch(FaultException<ServiceValidationFaultDetails> exception)
			{
				string error = string.Empty;

				if (exception.Message.Equals("Seat already exists", StringComparison.OrdinalIgnoreCase))
					error = ProjectResources.ResourceErrors.SeatExistsError;

				if (exception.Message.Equals("Area description isn't unique", StringComparison.OrdinalIgnoreCase))
					error = ProjectResources.ResourceErrors.AreaDescriptionError;

				return Json(new
				{
					success = false,
					errors = new string[] { error }
				});
			}
			catch (FaultException exception)
			{
				return Json(new
				{
					success = false,
					error = exception.Message
				});
			}

			return Json(new
            {
                success = true
            });
        }

        //GET: create new area view
        [HttpGet]
        public ActionResult CreateArea(int eventId)
        {
			ViewBag.Title = ProjectResources.EventResource.Create;
			ViewBag.Action = "CreateArea";
			return PartialView("~/Views/Event/Partial/_EventArea.cshtml", new EventAreaViewModel
            {
                SeatList = new List<EventSeat>(),
                EventId = eventId
            });
        }

		//GET: edit event view
        [HttpGet]
        public async Task<ActionResult> Edit()
        {
            ViewBag.VenueList = await _venueService.ToListAsync();
            return View();
        }

		//POST: edit event
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(EditEventViewModel model, HttpPostedFileBase image)
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
				
				var update = await _eventService.GetAsync(model.Event.Id);

				if (image != null && image.ContentLength > 0)
				{
					newImageUrl = GetImageUrl(image, out newImagePath);
					oldImageUrl = update.ImageURL;
				}

				update.Date = model.Event.Date.Date + model.Event.Time.TimeOfDay;
				update.Description = model.Event.Description;
				update.ImageURL = string.IsNullOrEmpty(newImageUrl) ? update.ImageURL : newImageUrl;
				update.LayoutId = model.Event.LayoutId;
				update.Title = model.Event.Title;
				await _eventService.UpdateAsync(update);

                if (!string.IsNullOrEmpty(newImageUrl))
                    image.SaveAs(newImagePath);
			}
			catch (FaultException<ServiceValidationFaultDetails> exception)
			{
				string error = string.Empty;

				if (exception.Message.Equals("Invalid date", StringComparison.OrdinalIgnoreCase))
					error = ProjectResources.ResourceErrors.EventInvalidDateError;

				if (exception.Message.Equals("Attempt of updating event with a date in the past", StringComparison.OrdinalIgnoreCase))
					error = ProjectResources.ResourceErrors.PastDateError;

				if (exception.Message.Equals("Not allowed to update layout. Event has locked seats", StringComparison.OrdinalIgnoreCase))
					error = ProjectResources.ResourceErrors.EventLayoutChangeError;

				return Json(new
				{
					success = false,
					errors = new string[] { error }
				});
			}
			catch (FaultException exception)
			{
				return Json(new
				{
					success = false,
					error = exception.Message
				});
			}

			return Json(new
			{
				success = true
			});
		}

        //get event by id to placeholder
		[NoDirectAccess]
		public async Task<ActionResult> GetEventToEdit(int eventId)
        {
            var data = await _eventService.GetEventInformationAsync(eventId);

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

			ViewBag.VenueList = await _venueService.ToListAsync();
			return PartialView("~/Views/Event/Partial/_GetEventToEdit.cshtml", new EditEventViewModel
            {
                Event = edit,
                EventAreas = data.Areas.ToList()
            });
        }

        [NoDirectAccess]
		public async Task<ActionResult> DeleteArea(int areaId)
		{
            try
            {
                await _eventAreaService.DeleteAsync(areaId);
				return Json(new
				{
					success = true
				});
            }
            catch (FaultException<ServiceValidationFaultDetails> exception)
            {
                string error = string.Empty;
                if (exception.Message.Equals("Not allowed to delete. Area has locked seat", StringComparison.OrdinalIgnoreCase))
                    error = ProjectResources.ResourceErrors.SeatLockedError;

                return Json(new
                {
                    success = false,
                    error
                });
            }
			catch (FaultException exception)
			{
				return Json(new
				{
					success = false,
					error = exception.Message
				});
			}
		}

		[NoDirectAccess]
		public async Task<ActionResult> DeleteEvent(int eventId)
		{
			try
			{
				await _eventService.DeleteAsync(eventId);
			}
			catch (FaultException<ServiceValidationFaultDetails> exception)
			{
				string error = string.Empty;

				if (exception.Message.Equals("Not allowed to delete. Event has locked seat", StringComparison.OrdinalIgnoreCase))
					error = ProjectResources.ResourceErrors.DeleteEventError;

				return Json(new
				{
					success = false,
					error
				});
			}
			catch (FaultException exception)
			{
				return Json(new
				{
					success = false,
					error = exception.Message
				});
			}

			return Json(new
			{
				success = true
			});
		}

        //get url of loaded image  and  path to save
        private string GetImageUrl(HttpPostedFileBase image, out string path)
		{
            var serverPath = ConfigurationManager.AppSettings["Uploads"];
            var directory = Server.MapPath(serverPath);
            
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

			string url;
			if (image != null && image.ContentLength > 0)
			{
				var fileName = Path.GetFileName(image.FileName);
				path = Path.Combine(directory, fileName);
				url = new Uri(Request.Url, Url.Content(serverPath + image.FileName)).ToString();
				return url;
			}
			else
			{
				url = new Uri(Request.Url, Url.Content(ConfigurationManager.AppSettings["DefaultEventImage"])).ToString();
				path = string.Empty;
				return url;
			}
		}
    }
}