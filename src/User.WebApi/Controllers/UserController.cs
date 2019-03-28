using System;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Exceptions;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User.WebApi.Infrastructure.AuthManager;
using User.WebApi.Infrastructure.Parsers;
using User.WebApi.Models;
using User.WebApi.Infrastructure.Hypermedia;
using System.Collections.Generic;
using BusinessLogic.Helpers;
using BusinessLogic.DTO;

namespace User.WebApi.Controllers
{
	[Authorize]
	public class UserController : Controller
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			this._userService = userService;
		}

		/// <summary>
		/// Get a user by id (access token is required to request this endpoint)
		/// </summary>
		/// <param name="id">User's id</param>
		/// <returns>UserModel</returns>
		/// <response code="200">Return a user model</response>
		/// <response code="400">Invalid client request</response>
		/// <response code="401">Authorization failed, no token passed or it expired</response>
		/// <response code="404">User not found</response>
		/// <response code="500">Internal server error</response>
		// GET api/user/{id}
		[HttpGet]
		[Route("api/user/{id}", Name = nameof(Get))]
		public async Task<IActionResult> Get(int id)
		{
			if (id <= 0)
				return BadRequest(new { Message = "Invalid client request" });

			try
			{
				var user = await this._userService.FindById(id);

				if (user is null)
					return NotFound(new { Message = "User not found" });

                var roles = await this._userService.GetRoles(user.UserName);

				var links = HypermediaHelper.GetUserHypermediaLinks(this, id);

                var userResult = UserParser.UserDtoToUserModel(user);
                userResult.Roles = roles.ToList();

                return Ok(new
				{
					User = userResult,
					Links = links
				});
			}
			catch (UserException exception)
			{
				return BadRequest(new { exception.Message });
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

		/// <summary>
		/// Get a user list (access token is required to request this endpoint)
		/// </summary>
		/// <returns>UserModel</returns>
		/// <response code="200">Return a user model</response>
		/// <response code="400">Invalid client request</response>
		/// <response code="401">Authorization failed, no token passed or it expired</response>
		/// <response code="404">User not found</response>
		/// <response code="500">Internal server error</response>
		// GET api/user/{id}
		[HttpGet]
		[Route("api/user", Name = nameof(GetUserList))]
		public async Task<IActionResult> GetUserList()
		{
			try
			{
				var users = await this._userService.GetList();

				var result = users.Select(x => UserParser.UserDtoToUserModel(x)).ToList();

				return Ok(new
				{
					Users = result
				});
			}
			catch (UserException exception)
			{
				return BadRequest(new { exception.Message });
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

        /// <summary>
        /// Create a user (access token is required to request this endpoint)
        /// </summary>
        /// <param name="model">UserModel</param>
        /// <response code="200">Success notification</response>
        /// <response code="400">Invalid client request</response>
        /// <response code="401">Authorization failed, no token passed or it expired</response>
        /// <response code="500">Internal server error</response>
        // POST api/user
        [Authorize(Roles = "Venue manager")]
        [HttpPost]
		[Route("api/user", Name = nameof(Post))]
		public async Task<IActionResult> Post([FromBody]CreateUpdateUserModel model)
		{
			if (model is null)
				return BadRequest(new { Message = "Invalid client request" });

			if (!ModelState.IsValid)
				return BadRequest(new { Message = ModelState.Values.SelectMany(x => x.Errors) });

			try
			{
				var user = UserParser.CreateUpdateUserModelToUserDto(model);

				await _userService.Create(user);
                await UpdateRoles(user, model.Roles);

				var links = HypermediaHelper.PostUserHypermediaLinks(this, user.Id);

				return Ok(new
				{
					Message = "User was created",
					Links = links
				});
			}
			catch (UserException exception)
			{
				return BadRequest(new { exception.Message });
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

		/// <summary>
		/// Update a user profile (access token is required to request this endpoint)
		/// </summary>
		/// <param name="id">User's id</param>
		/// <param name="model">UpdateUserModel</param>
		/// <response code="200">Success notification</response>
		/// <response code="400">Invalid client request</response>
		/// <response code="401">Authorization failed, no token passed or it expired</response>
		/// <response code="404">User not found</response>
		/// <response code="500">Internal server error</response>
		// PUT api/user/{id}/profile
		[HttpPut]
		[Route("api/user/{id}/profile", Name = nameof(UpdateProfile))]
		public async Task<IActionResult> UpdateProfile(int id, [FromBody]UpdateUserProfileModel model)
		{
			if (id <= 0 || model is null)
				return BadRequest(new { Message = "Invalid client request" });

			if (!ModelState.IsValid)
				return BadRequest(new { Message = ModelState.Values.SelectMany(x => x.Errors) });

			try
			{
				var user = await this._userService.Get(id);

				if (user == null)
					return NotFound(new { Message = "User not found" });

				user.Culture = model.Culture;
				user.Firstname = model.Firstname;
				user.Surname = model.Surname;
				user.Timezone = model.Timezone;

				if (!string.IsNullOrEmpty(model.Password) & !string.IsNullOrEmpty(model.NewPassword))
				{
					if (!PasswordHasher.VerifyHash(model.Password, user.Salt, user.PasswordHash))
						return BadRequest(new { Message = "Wrong current password" });

					var hash = PasswordHasher.ComputeHash(model.NewPassword);
					user.PasswordHash = hash.Hash;
					user.Salt = hash.Salt;
					await _userService.Update(user);

					return Ok(new { Message = "User was updated" });
				}

				await _userService.Update(user);

				var links = HypermediaHelper.PutUserHypermediaLinks(this, id);

				return Ok(new
				{
					Message = "User updated",
					Links = links
				});
			}
			catch (UserException exception)
			{
				return BadRequest(new { exception.Message });
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

		/// <summary>
		/// Delete a user (access token is required to request this endpoint)
		/// </summary>
		/// <param name="id">User's id</param>
		/// <response code="200">Success notification</response>
		/// <response code="400">Invalid client request</response>
		/// <response code="401">Authorization failed, no token passed or it expired</response>
		/// <response code="404">User not found</response>
		/// <response code="500">Internal server error</response>
		// DELETE api/user/{id}
		[Authorize(Roles = "Venue manager")]
		[HttpDelete]
		[Route("api/user/{id}", Name = nameof(Delete))]
		public async Task<IActionResult> Delete(int id)
		{
			if (id <= 0)
				return BadRequest(new { Message = "Invalid client request" });

			try
			{
				await _userService.Delete(id);

				return Ok(new { Message = "User was deleted" });
			}
			catch (UserException exception)
			{
				return BadRequest(new { exception.Message });
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

		/// <summary>
		/// Update balance of a user (access token is required to request this endpoint)
		/// </summary>
		/// <param name="id">User's id</param>
		/// <param name="model">UpdateUserBalanceModel</param>
		/// <response code="200">Success notification</response>
		/// <response code="400">Invalid client request</response>
		/// <response code="401">Authorization failed, no token passed or it expired</response>
		/// <response code="404">User not found</response>
		/// <response code="500">Internal server error</response>
		// PUT api/user/{id}/balance
		[HttpPut]
		[Route("api/user/{id}/balance", Name = nameof(UpdateUserBalance))]
		public async Task<IActionResult> UpdateUserBalance(int id, [FromBody]UpdateUserBalanceModel model)
		{
			if (id <= 0 || model.Amount < 0)
				return BadRequest(new { Message = "Invalid client request" });

			try
			{
				var user = await this._userService.Get(id);

				if (user == null)
					return NotFound(new { Message = "User not found" });

				user.Amount = model.Amount;
				await this._userService.Update(user);

				return Ok(new { Message = "Amount was updated" });
			}
			catch (UserException exception)
			{
				return BadRequest(new { exception.Message });
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

        /// <summary>
        /// Find a user by username(access token is required to request this endpoint)
        /// </summary>
        /// <param name="username"></param>
        /// <response code="200">Success notification</response>
        /// <response code="400">Invalid client request</response>
        /// <response code="401">Authorization failed, no token passed or it expired</response>
        /// <response code="404">User not found</response>
        /// <response code="500">Internal server error</response>
        // PUT api/user/{id}
        [HttpGet]
		[Route("api/user/search/username/{username}", Name = nameof(FindByUserName))]
		public async Task<IActionResult> FindByUserName(string username)
		{
			if (string.IsNullOrEmpty(username))
				return BadRequest(new { Message = "Invalid client request" });

			try
			{
				var user = await this._userService.FindByUserName(username);

				if (user == null)
					return NotFound(new { Message = "User not found" });

				var roles = await this._userService.GetRoles(user.UserName);

				var links = HypermediaHelper.GetUserHypermediaLinks(this, user.Id);

				var userResult = UserParser.UserDtoToUserModel(user);
				userResult.Roles = roles.ToList();

				return Ok(new
				{
					User = userResult,
					Links = links
				});
			}
			catch (UserException exception)
			{
				return BadRequest(new { exception.Message });
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

		/// <summary>
		/// Find a user by email (access token is required to request this endpoint)
		/// </summary>
		/// <param name="email"></param>
		/// <response code="200">Success notification</response>
		/// <response code="400">Invalid client request</response>
		/// <response code="401">Authorization failed, no token passed or it expired</response>
		/// <response code="404">User not found</response>
		/// <response code="500">Internal server error</response>
		// PUT api/user/{id}
		[HttpGet]
		[Route("api/user/search/email/{email}", Name = nameof(FindByEmail))]
		public async Task<IActionResult> FindByEmail(string email)
		{
			if (string.IsNullOrEmpty(email))
				return BadRequest(new { Message = "Invalid client request" });

			try
			{
				var user = await this._userService.FindByEmail(email);

				if (user == null)
					return NotFound(new { Message = "User not found" });

				var roles = await this._userService.GetRoles(user.UserName);

				var links = HypermediaHelper.GetUserHypermediaLinks(this, user.Id);

				var userResult = UserParser.UserDtoToUserModel(user);
				userResult.Roles = roles.ToList();

				return Ok(new
				{
					User = userResult,
					Links = links
				});
			}
			catch (UserException exception)
			{
				return BadRequest(new { exception.Message });
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

		/// <summary>
		/// Update a user including roles (access token is required to request this endpoint)
		/// </summary>
		/// <param name="id">user's id</param>
		/// <param name="model"></param>
		/// <response code="200">Success notification</response>
		/// <response code="400">Invalid client request</response>
		/// <response code="401">Authorization failed, no token passed or it expired</response>
		/// <response code="404">User not found</response>
		/// <response code="500">Internal server error</response>
		// PUT api/user/{id}
		[Authorize(Roles = "Venue manager")]
		[HttpPut]
		[Route("api/user/{id}", Name = nameof(Put))]
		public async Task<IActionResult> Put(int id, [FromBody]CreateUpdateUserModel model)
		{
			if (id <= 0 || model is null)
				return BadRequest(new { Message = "Invalid client request" });

			if (!ModelState.IsValid)
				return BadRequest(new { Message = ModelState.Values.SelectMany(x => x.Errors) });

			try
			{
				var user = await this._userService.Get(id);

				if (user == null)
					return NotFound(new { Message = "User not found" });

				var links = HypermediaHelper.PutUserHypermediaLinks(this, id);

				user.Culture = model.Culture;
				user.Firstname = model.Firstname;
				user.Surname = model.Surname;
				user.Timezone = model.Timezone;
				user.Email = model.Email;
				user.UserName = model.UserName;

				if (!string.IsNullOrEmpty(model.Password))
				{
					var hash = PasswordHasher.ComputeHash(model.Password);
					user.PasswordHash = hash.Hash;
					user.Salt = hash.Salt;
				}

				await _userService.Update(user);
				await UpdateRoles(user, model.Roles);

				return Ok(new
				{
					Message = "User was updated",
					Links = links
				});
			}
			catch (UserException exception)
			{
				return BadRequest(new { exception.Message });
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

		private async Task UpdateRoles(UserDto user, IEnumerable<string> roles)
		{
			var currentRoles = await _userService.GetRoles(user.UserName);

			//add roles
			foreach (var role in roles)
			{
				var roleEnumValue = FindRoleEnumValueByDescription(role);

				if (roleEnumValue is null)
					throw new InvalidOperationException("Role does not exists");

				if (currentRoles.Any(x => x.Equals(role)))
					continue;

				await _userService.AddRole(user, (Role)roleEnumValue);
			}

			//Find roles which have been deleted
			var difference = currentRoles.Where(list2 => roles.All(list1 => !list2.Equals(list1, StringComparison.OrdinalIgnoreCase))).ToList();
			foreach (var role in difference)
			{
				var roleEnumValue = FindRoleEnumValueByDescription(role);
				await _userService.DeleteRole(user, (Role)roleEnumValue);
			}
		}

		private Role? FindRoleEnumValueByDescription(string str)
		{
			return Enum.GetValues(typeof(Role)).Cast<Role>().
				Where(x => GetEnumItemDescription.GetEnumDescription(x).Equals(str, StringComparison.OrdinalIgnoreCase))
				.SingleOrDefault();
		}
	}
}
