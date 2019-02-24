using BusinessLogic.DTO;
using BusinessLogic.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using User.WebApi.Models;

namespace User.WebApi.Infrastructure.AuthManager
{
	internal class AuthManager : IAuthManager
	{
		private readonly IUserService _userService;
		private readonly IConfiguration _configuration;

		public AuthManager(IUserService userService, IConfiguration configuration)
		{
			this._userService = userService;
			this._configuration = configuration;
		}

		public async Task<TokenModel> GenerateToken(UserDto user)
		{
			var secretKey = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(_configuration.GetSection("JWTTokenCredentials:SecretKey").Value));
			var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

			var roles = await _userService.GetRoles(user.UserName);

			var claims = new List<Claim>();

			roles.ToList().ForEach(x =>
			{
				claims.Add(new Claim(ClaimTypes.Role, x));
			});

			claims.Add(new Claim(ClaimTypes.Name, user.UserName));
			claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
			claims.Add(new Claim(ClaimTypes.Email, user.Email));
			claims.Add(new Claim("AccountBalance", user.Amount.ToString(CultureInfo.InvariantCulture)));
			claims.Add(new Claim("Culture", user.Culture));

			var tokenOptions = new JwtSecurityToken(
					issuer: _configuration.GetSection("JWTTokenCredentials:Issuer").Value,
					audience: _configuration.GetSection("JWTTokenCredentials:Audience").Value,
					claims: claims,
					expires: DateTime.UtcNow.AddMinutes(30),
					signingCredentials: signinCredentials);

            return new TokenModel
            {
                RefreshToken = GenerateRefreshToken(),
                Token = new JwtSecurityTokenHandler().WriteToken(tokenOptions)
            };
		}

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private Task<ClaimsPrincipal> GetPrincipalFromToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWTTokenCredentials:SecretKey").Value)),
				ValidIssuer = _configuration.GetSection("JWTTokenCredentials:Issuer").Value,
				ValidAudience = _configuration.GetSection("JWTTokenCredentials:Audience").Value,
				ValidateLifetime = false 
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.
                Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return Task.FromResult(principal);
        }

        public async Task<TokenModel> RefreshToken(TokenModel tokenModel)
        {
            var principal = await GetPrincipalFromToken(tokenModel.Token);

            int.TryParse(principal.Claims.FirstOrDefault(x => x.Type is ClaimTypes.NameIdentifier).Value, out var userId);

            var userRefreshToken = await _userService.GetRefreshToken(userId);

            if (!tokenModel.RefreshToken.Equals(userRefreshToken, StringComparison.Ordinal))
                throw new SecurityTokenException("Invalid refresh token");

            var secretKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetSection("JWTTokenCredentials:SecretKey").Value));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                    issuer: _configuration.GetSection("JWTTokenCredentials:Issuer").Value,
                    audience: _configuration.GetSection("JWTTokenCredentials:Audience").Value,
                    claims: principal.Claims,
                    expires: DateTime.Now.AddMinutes(60),
                    signingCredentials: signinCredentials);

            var newJwt = new TokenModel
            {
                RefreshToken = GenerateRefreshToken(),
                Token = new JwtSecurityTokenHandler().WriteToken(tokenOptions)
            };

            await _userService.SetRefreshToken(userId, newJwt.RefreshToken);

            return newJwt;
        }
    }
}
