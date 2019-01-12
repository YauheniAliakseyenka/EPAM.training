using BusinessLogic.Exceptions;
using BusinessLogic.Services;
using Hangfire;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TicketManagementMVC.Helpers;
using TicketManagementMVC.Infrastructure.Attributes;
using TicketManagementMVC.Infrastructure.Authentication;
using TicketManagementMVC.Infrastructure.BackgroundWorker;
using TicketManagementMVC.Models;

namespace TicketManagementMVC.Controllers
{
	public class AccountController : Controller
    {
		private IAuthenticationManager _authManager => HttpContext.GetOwinContext().Authentication;
		private ApplicationUserManager _userManager;
		private ICartService _cartService;
		private IOrderService _orderService;
		private IEmailService _emailService;
		private ISeatLocker _seatLocker;

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

		public AccountController(ApplicationUserManager userManager, ICartService cartService,  
			IOrderService orderService, IEmailService emailService, ISeatLocker seatLocker)
		{
			_userManager = userManager;
			_cartService = cartService;
			_orderService = orderService;
			_emailService = emailService;
			_seatLocker = seatLocker;
		}

        //GET: registration view
        [HttpGet]
		[AllowAnonymous]
		public ActionResult Registration()
		{
			if (User.Identity.IsAuthenticated)
				return RedirectToAction("Index", "Home");

			return View(new RegistrationViewModel());
		}

        //POST: login
        [HttpPost]
        [AllowAnonymous]
		[ValidateAntiForgeryToken]
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

        //create identity to login
		private async Task<ModelStateDictionary> SignIn(string userName, string password)
		{
			var user = await _userManager.FindAsync(userName, password);
			if (user == null)
			{
				ModelState.AddModelError(string.Empty, I18N.ResourceErrors.LoginError);
				return ModelState;
			}

			var identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
			CultureSetter.Set(user.Culture, this);
			_authManager.SignOut();
			_authManager.SignIn(identity);
			return ModelState;
		}

		[Authorize]
		public ActionResult Logout()
		{
			_authManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

			return RedirectToAction("Index", "Home");
		}

        //POST: registration
        [HttpPost]
        [AllowAnonymous]
		[ValidateAntiForgeryToken]
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
			
			var insert = await _userManager.CreateAsync(user, model.Password);

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

        //GET: cart view
        [Authorize]
		[HttpGet]
		public async Task<ActionResult> Cart()
		{
			return View(await _cartService.GetOrderedSeats(User.Identity.GetUserId()));
		}

        //seat's deleting from cart 
        [NoDirectAccess]
		public async Task DeleteFromCart(int seatId)
		{
			await _seatLocker.UnlockSeat(seatId);
		}

        //seat's adding to cart 
        [NoDirectAccess]
		public async Task<ActionResult> AddSeatToCart(int seatId)
		{
			try
			{
				await _seatLocker.LockSeat(seatId, User.Identity.GetUserId());
				
			}
			catch (CartException exception)
			{
				string error = string.Empty;

				if (exception.Message.Equals("Seat is locked", StringComparison.OrdinalIgnoreCase))
					error = I18N.ResourceErrors.SeatLocked;

				return Json(new
				{
					success = false,
					error
				});
			}

			return Json(new
			{
				success = true,
				message = I18N.Resource.AddedToCartNotify
			});
		}

        //complete order
		[Authorize]
		public async Task<ActionResult> Order()
		{
			List<int> seatIdList;
			try
			{
				_orderService.Ordered += _emailService.Send;

				var result = await _orderService.Create(User.Identity.GetUserId());
				seatIdList = result.ToList();				
			}
			catch (OrderException exception)
			{
				string error = exception.Message;

				if (exception.Message.Equals("Balance of user is less than total amount of order", StringComparison.OrdinalIgnoreCase))
					error = I18N.ResourceErrors.OrderError;

				if (exception.Message.Equals("User has no ordered seats", StringComparison.OrdinalIgnoreCase))
					error = I18N.ResourceErrors.OrderedSeatError;

				return Json(new
				{
					success = false,
					error
				});
			}

			seatIdList.ForEach(x =>
			{
				RecurringJob.RemoveIfExists("unlockSeatId" + x);
			});

			return Json(new
			{
				success = true
			});
		}

        //GET: purchase history view
		[HttpGet]
		[Authorize]
		public async Task<ActionResult> PurchaseHistory()
		{
			var model = await _orderService.GetPurchaseHistory(User.Identity.GetUserId());

			return View(model);
		}

        //GET: user profile view
        [HttpGet]
		public async Task<ActionResult> UserProfile()
		{
			if (!User.Identity.IsAuthenticated)
				return RedirectToAction("Registration", "Account");

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

        //POST: user profile's saving
        [HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> UserProfile(UserProfileViewModel model)
		{
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
			CultureSetter.Set(user.Culture, this);
			
			return RedirectToAction("Index", "Home");
		}

        //GET: replenishment of balance view
        [HttpGet]
		[Authorize]
		public ActionResult BalanceReplenishment()
		{
			return View(new BalanceReplenishmentViewModel());
		}

        //GET: replenishment of balance
        [HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
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