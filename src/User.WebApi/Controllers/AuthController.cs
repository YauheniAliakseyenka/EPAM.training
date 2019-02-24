using System;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Exceptions;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using User.WebApi.Infrastructure.AuthManager;
using User.WebApi.Infrastructure.Parsers;
using User.WebApi.Models;
using User.WebApi.Infrastructure.Hypermedia;

namespace User.WebApi.Controllers
{
	[Route("api/authenticate")]
	public class AuthController : Controller
	{
		private readonly IAuthManager _userManager;
		private readonly IUserService _userService;

        public AuthController(IAuthManager userManager, IUserService userService)
		{
			this._userManager = userManager;
			this._userService = userService;
		}

        /// <summary>
        /// Get a token by user credentials
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns>A pair of tokens: access jwt token and refresh token (TokenModel)</returns>
        /// <response code="200">Returns a pair of tokens</response>
        /// <response code="400">Invalid client request</response>
        /// <response code="404">User not found</response>
        /// <response code="500">Internal server error</response>
        // POST api/authenticate/token
        [AllowAnonymous]
		[HttpPost]
		[Route("token")]
		public async Task<IActionResult> Token([FromBody]LoginModel credentials)
		{
			if (credentials == null || !ModelState.IsValid)
			{
				return BadRequest(new { Message = "Invalid client request" });
			}

			try
			{
				var user = await this._userService.FindByUserName(credentials.UserName);

				if (user == null)
					return NotFound(new { Message = "User not found" });

				TokenModel token = null;
				if (PasswordHasher.VerifyHash(credentials.Password, user.Salt, user.PasswordHash))
				{
					token = await this._userManager.GenerateToken(user);
				}
				else
					return BadRequest(new { Message = "Wrong current password" });

				await _userService.SetRefreshToken(user.Id, token.RefreshToken);

				//add hypermedia
				var links = HypermediaHelper.AuthHypermediaLinks(this);

                return Ok(new { token, links });
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
        /// Regsitration of a user
        /// </summary>
        /// <param name="model"></param>
        /// <returns>A pair of tokens: access jwt token and refresh token (TokenModel)</returns>
        /// <response code="200">Returns a pair of tokens</response>
        /// <response code="400">Invalid client request</response>
        /// <response code="500">Internal server error</response>
        // POST api/authenticate/registration
        [AllowAnonymous]
		[HttpPost]
		[Route("registration")]
		public async Task<IActionResult> Register([FromBody]RegistratationUserModel model)
		{
			if (model is null)
				return BadRequest(new { Message = "Invalid client request" });

			if (!ModelState.IsValid)
				return BadRequest(new { Message = ModelState.Values.SelectMany(x => x.Errors) });

			try
			{
				var user = UserParser.RegistratationUserModelToUserDto(model);

				await _userService.Create(user);

				var token = await this._userManager.GenerateToken(user);
				await _userService.SetRefreshToken(user.Id, token.RefreshToken);

                //add hypermedia
                var links = HypermediaHelper.AuthHypermediaLinks(this);

                return Ok(new { token, links });
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
        /// Refresh a token
        /// </summary>
        /// <param name="model"></param>
        /// <returns>A pair of tokens: access jwt token and refresh token (TokenModel)</returns>
        /// <response code="200">Returns a pair of tokens</response>
        /// <response code="400">Invalid client request</response>
        /// <response code="500">Internal server error</response>
        // POST api/authenticate/refresh-token
        [AllowAnonymous]
        [HttpPut]
        [Route("token", Name = nameof(RefreshToken))]
        public async Task<IActionResult> RefreshToken([FromBody]TokenModel model)
        {
            if (model is null || !ModelState.IsValid)
                return BadRequest(new { Message = "Invalid client request" });

            try
            {
                var token = await _userManager.RefreshToken(model);

                return Ok(token);
            }
            catch (SecurityTokenException exception)
            {
                return BadRequest(new { exception.Message });
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
