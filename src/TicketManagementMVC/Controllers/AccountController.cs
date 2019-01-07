using BusinessLogic;
using BusinessLogic.Exceptions;
using BusinessLogic.Services;
using Hangfire;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TicketManagementMVC.Infrastructure;
using TicketManagementMVC.Infrastructure.Authentication;
using TicketManagementMVC.Models;

namespace TicketManagementMVC.Controllers
{
	public class AccountController : Controller
    {
		private IAuthenticationManager _authManager => HttpContext.GetOwinContext().Authentication;
		private UserManager<User, string> _userManager;
		private ICartService _cartService;
		private IOrderService _orderService;
		private IEmailService _emailService;

		protected override void Initialize(RequestContext requestContext)
		{
			base.Initialize(requestContext);
			var identity = HttpContext.User.Identity;
			if (identity.IsAuthenticated)
			{
				var user = _userManager.FindByName(identity.GetUserName());
				Thread.CurrentThread.CurrentCulture = new CultureInfo(user.Culture);
				Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
				ViewData["Balance"] = string.Format("${0:N2}", user.Amount);
			}
		}

		public AccountController(UserManager<User, string> userManager, ICartService cartService,  
			IOrderService orderService, IEmailService emailService)
		{
			_userManager = userManager;
			_cartService = cartService;
			_orderService = orderService;
			_emailService = emailService;

			_userManager.UserValidator = new UserValidator<User>(_userManager)
			{
				AllowOnlyAlphanumericUserNames = false,
				RequireUniqueEmail = true
			};
		}
		
		[HttpGet]
		[AllowAnonymous]
		public ActionResult Registration()
		{
			if (User.Identity.IsAuthenticated)
				return RedirectToAction("Index", "Home");

			return View(new RegistrationViewModel());
		}

		[HttpGet]
		[AllowAnonymous]
		public ActionResult Login()
		{
			if (User.Identity.IsAuthenticated)
				return RedirectToAction("Index", "Home");

			return View(new LoginViewModel());
		}

		[HttpPost]
		public async Task<ActionResult> Login(LoginViewModel model)
		{
			if(string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
			{
				ModelState.AddModelError(string.Empty, I18N.ResourceErrors.LoginError);
				return Json(new
				{
					success = false,
					errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
								.Select(m => m.ErrorMessage).ToArray()
				});
			}

			var login = await SignIn(model.UserName, model.Password);
			if (!login.IsValid)
				return Json(new
				{
					success = false,
					errors = login.Keys.SelectMany(k => ModelState[k].Errors)
									.Select(m => m.ErrorMessage).ToArray()
				});


			return Json(new
			{
				success = true
			});
		}

		private async Task<ModelStateDictionary> SignIn(string userName, string password)
		{
			var user = await _userManager.FindAsync(userName, password);
			if (user == null)
			{
				ModelState.AddModelError(string.Empty, I18N.ResourceErrors.LoginError);
				return ModelState;
			}

			var identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
			_authManager.SignOut();
			_authManager.SignIn(identity);
			return ModelState;
		}

		[CustomAuthorize]
		public ActionResult Logout()
		{
			_authManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

			return RedirectToAction("Index", "Home");
		}

		[HttpPost]
		public async Task<ActionResult> Registration(RegistrationViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			var user = new User
			{
				UserName = model.UserName,
				PasswordHash = model.Password,
				Surname = model.Surname,
				Culture = model.Culture,
				Email = model.Email,
				Firstname = model.Firstname,
				Timezone = model.SelectedTimezone
			};


			var insert = _userManager.Create(user, model.Password);

			if (insert.Errors.Any())
			{
				foreach (var error in insert.Errors)
				{
					string errorMessage = error;
					if (error.StartsWith("Name") && error.EndsWith("is already taken."))
						errorMessage = I18N.ResourceErrors.UserNameIsTaken;

					if (error.StartsWith("Email") && error.EndsWith("is already taken."))
						errorMessage = I18N.ResourceErrors.EmalIsTaken;

					ModelState.AddModelError(string.Empty, errorMessage);
				}

				return View();
			}

			await SignIn(model.UserName, model.Password);

			return RedirectToAction("Index", "Home");
		}

		[CustomAuthorize]
		public ActionResult Cart()
		{
			if (!User.Identity.IsAuthenticated)
				return RedirectToAction("Index", "Home");

			return View(_cartService.GetOrderedSeats(User.Identity.GetUserId()));
		}
		
		[CustomAuthorize]
		[NoDirectAccess]
		public void DeleteFromCart(int seatId)
		{
			RecurringJob.Trigger("unlockSeatId" + seatId);
		}

		[CustomAuthorize]
		[HttpPost]
		public ActionResult CompleteOrder()
		{
			try
			{
				var seatIds = _cartService.GetOrderedSeats(User.Identity.GetUserId()).
				Select(x => x.Seat).
				Select(x => x.Id).ToList();

				_orderService.OrderCompleted += _emailService.Send;
				_orderService.Create(User.Identity.GetUserId());

				seatIds.ForEach(x =>
				{
					RecurringJob.RemoveIfExists("unlockSeatId" + x);
				});
			}
			catch (OrderException exception)
			{
				string error = exception.Message;

				if (exception.Message.Equals("Balance of user less than total amount of order", StringComparison.OrdinalIgnoreCase))
					error = I18N.ResourceErrors.OrderError;

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

		[HttpGet]
		[CustomAuthorize]
		public ActionResult PurchaseHistory()
		{
			if (!User.Identity.IsAuthenticated)
				return RedirectToAction("Index", "Home");

			return View(_orderService.GetPurchaseHistory(User.Identity.GetUserId()).ToList());
		}

		[HttpGet]
		[CustomAuthorize]
		public async Task<ActionResult> UserProfile()
		{
			if (!User.Identity.IsAuthenticated)
				return RedirectToAction("Index", "Home");

			var user = await _userManager.FindByNameAsync(User.Identity.GetUserName());

			return View(new UserProfileViewModel
			{
				Surname = user.Surname,
				Culture = user.Culture,
				Email = user.Email,
				Firstname = user.Firstname,
				Timezone = user.Timezone,
			});
		}

		[HttpPost]
		[CustomAuthorize]
		public async Task<ActionResult> UserProfile(UserProfileViewModel model)
		{
			if (!User.Identity.IsAuthenticated)
				return RedirectToAction("Registration", "Account");

			var user = await _userManager.FindByNameAsync(User.Identity.GetUserName());

			if (!string.IsNullOrEmpty(model.Password))
			{
				//check current password
				if (_userManager.PasswordHasher.VerifyHashedPassword(user.PasswordHash, model.Password).Equals(PasswordVerificationResult.Failed))
				{
					ModelState.AddModelError(string.Empty, I18N.ResourceErrors.ChangePasswordError);
					return View();
				}

				if ((string.IsNullOrEmpty(model.NewPassword) | string.IsNullOrEmpty(model.ConfirmNewPassword)) || !ModelState.IsValid)
					return View();

				user.PasswordHash = _userManager.PasswordHasher.HashPassword(model.NewPassword);
			}
			else
			{
				if (!ModelState.IsValid)
					return View();
			}

			user.Surname = model.Surname;
			user.Culture = model.Culture;
			user.Email = model.Email;
			user.Firstname = model.Firstname;
			user.Timezone = model.Timezone;			
			var update = await _userManager.UpdateAsync(user);

			if (update.Errors.Any())
			{
				update.Errors.ToList().ForEach(x =>
				{
					ModelState.AddModelError(string.Empty, x);
				});
				return View();
			}


			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		[CustomAuthorize]
		public ActionResult BalanceReplenishment()
		{
			return View(new BalanceReplenishmentViewModel());
		}

		[HttpPost]
		[CustomAuthorize]
		public async Task<ActionResult> BalanceReplenishment(BalanceReplenishmentViewModel model)
		{
            if (!ModelState.IsValid)
                return View();

			var user = await _userManager.FindByIdAsync(User.Identity.GetUserId());
			user.Amount += model.Amount;
			var update = await _userManager.UpdateAsync(user);

			if (update.Errors.Any())
			{
				update.Errors.ToList().ForEach(x =>
				{
					ModelState.AddModelError(string.Empty, x);
				});
				return View();
			}

			return RedirectToAction("Index", "Home");
		}
	}
}