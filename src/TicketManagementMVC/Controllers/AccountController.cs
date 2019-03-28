using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using TicketManagementMVC.Helpers;
using TicketManagementMVC.Infrastructure.Attributes;
using TicketManagementMVC.Infrastructure.Authentication;
using TicketManagementMVC.Models;
using TicketManagementMVC.PurchaseService;
using TicketManagementMVC.Infrastructure.Helpers.Parsers;
using System.ServiceModel;
using WcfBusinessLogic.Core.Contracts.Exceptions;

namespace TicketManagementMVC.Controllers
{
	public class AccountController : Controller
    {
		private AuthManager _authManager => new AuthManager(this.HttpContext);
		private IWcfPurchaseService _purchaseService;
        private CustomUserManager _userManager;

		private readonly int _purchaseHistoryPageSize;

		protected override void Initialize(RequestContext requestContext)
		{
			base.Initialize(requestContext);
			var identity = requestContext.HttpContext.User;
			if (identity.Identity.IsAuthenticated)
				ViewData["Balance"] = _authManager.GetAccountBalance();		
		}

		public AccountController(IWcfPurchaseService purchaseService, CustomUserManager userManager)
		{
            _userManager = userManager;
			_purchaseService = purchaseService;
			_purchaseHistoryPageSize = 5;
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
            if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
            {
                ModelState.AddModelError(string.Empty, ProjectResources.ResourceErrors.LoginError);
                return Json(new
                {
                    success = false,
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                                .Select(m => m.ErrorMessage).ToArray()
                });
            }

			var response = await _userManager.CreateIdentity(model);

            if(response.IsSuccess)
            {
                _authManager.SignOut();
                _authManager.SignIn((ClaimsIdentity)response.Object, out var culture);
				CultureSetter.Set(culture, this);

                return Json(new
                {
                    success = true
                });
            }

            return Json(new
            {
                success = false,
                errors = new string[] { ProjectResources.ResourceErrors.LoginError }
            });
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
                return Json(new
                {
                    success = false,
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                                  .Select(m => m.ErrorMessage).ToArray()
                });
            }

			var user = UserParser.RegistrationViewModelToUser(model);
			var response = await _userManager.CreateIdentity(user);

            if (response.IsSuccess)
            {
                _authManager.SignIn((ClaimsIdentity)response.Object);
				CultureSetter.Set(model.Culture, this);

				return Json(new
                {
                    success = true
                });
            }

            string error = string.Empty;
            if (response.Message.Equals("Username is already taken", StringComparison.OrdinalIgnoreCase))
                error = ProjectResources.ResourceErrors.UserNameIsTaken;

            if (response.Message.Equals("Email is already taken", StringComparison.OrdinalIgnoreCase))
                error = ProjectResources.ResourceErrors.EmailIsTaken;


            return Json(new
            {
                success = false,
                errors = new string[] { error }
            });
        }

        //GET: cart view
        [Authorize]
		[HttpGet]
		public async Task<ActionResult> Cart()
		{
			return View(await _purchaseService.GetOrderedSeatsAsync(User.Identity.GetUserId<int>()));
		}

		//seat's deleting from cart 
		[NoDirectAccess]
		public async Task DeleteFromCart(int seatId)
		{
			await _purchaseService.DeleteSeatFromCartAsync(seatId);
		}

        //seat's adding to cart 
        [NoDirectAccess]
		public async Task<ActionResult> AddSeatToCart(int seatId)
		{
			try
			{
				await _purchaseService.AddSeatToCartAsync(seatId, User.Identity.GetUserId<int>());
			}
			catch (FaultException<ServiceValidationFaultDetails> exception)
			{
				string error = exception.Message;

				if (exception.Message.Equals("Seat is locked", StringComparison.OrdinalIgnoreCase))
					error = ProjectResources.ResourceErrors.SeatLocked;

				return Json(new
				{
					success = false,
					error
				});
			}
			catch(FaultException exception)
			{
				return Json(new
				{
					success = false,
					error = exception.Message
				});
			}

			return Json(new
			{
				success = true,
				message = ProjectResources.AccountResource.AddedToCartNotify
			});
		}

        //complete order
		[Authorize]
		public async Task<ActionResult> Order()
		{
			try
			{
				await _purchaseService.CreateOrderAsync(User.Identity.GetUserId<int>());
				var response = await _userManager.UpdateAmount(this.User.Identity as ClaimsIdentity, BalanceUpdateVerb.UpdateFromServer);

				if (!response.IsSuccess)
					return Json(new
					{
						success = false,
						error = response.Message
					});
			}
			catch (FaultException<ServiceValidationFaultDetails> exception)
			{
				string error = exception.Message;

				if (exception.Message.Equals("Balance of user is less than total amount of order", StringComparison.OrdinalIgnoreCase))
					error = ProjectResources.ResourceErrors.OrderError;

				if (exception.Message.Equals("User has no ordered seats", StringComparison.OrdinalIgnoreCase))
					error = ProjectResources.ResourceErrors.OrderedSeatError;

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

        //GET: get first page of purchase history
		[HttpGet]
		[Authorize]
		public async Task<ActionResult> PurchaseHistory(int index = 0)
		{
			var data = await _purchaseService.GetPurchaseHistoryAsync(User.Identity.GetUserId<int>());
			ViewBag.Count = data.Count();
			ViewBag.PageSize = _purchaseHistoryPageSize;
			ViewBag.CurrentIndex = index;
			return View(data.Skip(index).Take(_purchaseHistoryPageSize));
		}

		//GET: user profile view
		[HttpGet]
		[Authorize]
		public async Task<ActionResult> UserProfile()
		{
			if (!User.Identity.IsAuthenticated)
				return RedirectToAction("Registration", "Account");

            var response = await _userManager.GetUser(this.User.Identity as ClaimsIdentity);
            var user = (Infrastructure.Authentication.User)response.Object;

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
			var user = UserParser.UserProfileViewModelToUser(model);

			if (!string.IsNullOrEmpty(model.Password))
			{

				if ((string.IsNullOrEmpty(model.NewPassword) | string.IsNullOrEmpty(model.ConfirmNewPassword)) || !ModelState.IsValid)
					return Json(new
					{
						success = false,
						errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
								 .Select(m => m.ErrorMessage).ToArray()
					});

				user.Password = model.Password;
				user.NewPassword = model.NewPassword;
			}
			else
			{
				if (!ModelState.IsValid)
					return Json(new
					{
						success = false,
						errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
								 .Select(m => m.ErrorMessage).ToArray()
					});
			}
			
			var response = await _userManager.Update(user, (ClaimsIdentity)this.User.Identity);

            if(response.IsSuccess)
            {
                CultureSetter.Set(model.Culture, this);

                return Json(new
                {
                    success = true
                });
            }

			if (response.Message.Equals("Wrong current password", StringComparison.OrdinalIgnoreCase))
				return Json(new
				{
					success = false,
					errors = new string[] { ProjectResources.ResourceErrors.ChangePasswordError }
				});

            return Json(new
            {
                success = false,
                errors = new string[] { response.Message }
            });
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
				return Json(new
				{
					success = false,
					errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
								  .Select(m => m.ErrorMessage).ToArray()
				});

			var response = await _userManager.UpdateAmount(this.User.Identity as ClaimsIdentity, BalanceUpdateVerb.Replenishment, model.Amount);

			if (response.IsSuccess)
				return Json(new
				{
					success = true
				});

			return Json(new
			{
				success = false,
				errors = new string[] { response.Message }
			});
		}

		//cancel order and refund money
		[Authorize]
		[NoDirectAccess]
		public async Task Refund(int orderId)
		{
			await _purchaseService.CancelOrderAndRefundAsync(orderId);
			await _userManager.UpdateAmount(this.User.Identity as ClaimsIdentity, BalanceUpdateVerb.UpdateFromServer);
		}
	}
}