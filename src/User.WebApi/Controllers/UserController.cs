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

namespace User.WebApi.Controllers
{
	[Authorize]
	[Route("api/user")]
	public class UserController : Controller
	{
		private readonly IUserService _userService;

        public UserController(IUserService userService)
		{
			this._userService = userService;
		}

		/// <summary>
		/// Get a user by id (access token required)
		/// </summary>
		/// <param name="id"></param>
		/// <returns>UserModel</returns>
		/// <response code="200">Return a user model</response>
		/// <response code="400">Invalid client request</response>
		/// <response code="401">Authorization failed, no token passed or it expired</response>
		/// <response code="404">User not found</response>
		/// <response code="500">Internal server error</response>
		// GET api/user/{id}
		[HttpGet("{id}", Name = nameof(Get))]
		public async Task<IActionResult> Get(int id)
		{
            if (id <= 0)
                return BadRequest(new { Message = "Invalid client request" });

			try
			{
				var user = await this._userService.FindById(id);

				if (user is null)
					return NotFound(new { Message = "User not found" });

                var links = HypermediaHelper.GetUserHypermediaLinks(this, id);

				return Ok(new
                {
                    User = UserParser.UserDtoToUserModel(user),
                    Links = links
                });
			}
			catch(UserException exception)
			{
                return BadRequest(new { exception.Message });
			}
			catch(Exception)
			{
				return StatusCode(500);
			}
		}

		/// <summary>
		/// Create a user (access token required)
		/// </summary>
		/// <param name="model"></param>
		/// <response code="200">Success notification</response>
		/// <response code="400">Invalid client request</response>
		/// <response code="401">Authorization failed, no token passed or it expired</response>
		/// <response code="500">Internal server error</response>
		// POST api/user
		[Authorize(Roles = "Venue manager")]
		[HttpPost("{id}", Name = nameof(Post))]
		public async Task<IActionResult> Post([FromBody]UserModel model)
		{
            if (model is null)
                return BadRequest(new { Message = "Invalid client request" });

            if (!ModelState.IsValid)
                return BadRequest(new { Message = ModelState.Values.SelectMany(x => x.Errors) });

            try
            {
                var user = UserParser.UserModelToUserDto(model);

                var hashedPassword = PasswordHasher.ComputeHash(model.Password);
                user.PasswordHash = hashedPassword.Hash;
                user.Salt = hashedPassword.Salt;

                await _userService.Create(user);

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
		/// Update a user (access token required)
		/// </summary>
		/// <param name="id"></param>
		/// <param name="model"></param>
		/// <response code="200">Success notification</response>
		/// <response code="400">Invalid client request</response>
		/// <response code="401">Authorization failed, no token passed or it expired</response>
		/// <response code="404">User not found</response>
		/// <response code="500">Internal server error</response>
		// PUT api/user/{id}
		[HttpPut("{id}", Name = nameof(Put))]
		public async Task<IActionResult> Put(int id, [FromBody]UpdateUserModel model)
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

				user.Amount = model.Amount;
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
		/// Delete a user (access token required)
		/// </summary>
		/// <param name="id"></param>
		/// <response code="200">Success notification</response>
		/// <response code="400">Invalid client request</response>
		/// <response code="401">Authorization failed, no token passed or it expired</response>
		/// <response code="404">User not found</response>
		/// <response code="500">Internal server error</response>
		// DELETE api/user/{id}
		[Authorize(Roles = "Venue manager")]
		[HttpDelete("{id}", Name = nameof(Delete))]
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
	}
}
