using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DAL.Entities.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using WebUI.Helpers;
using WebUI.Models;
using WebUI.Models.AccountViewModels;
using WebUI.Services;

namespace WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger _logger;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpPost("/token")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Provide correct data, please!");
            }
            var username = model.UserName;
            var password = model.Password;
            var response = await GetAuthorizationResponse(username, password);
            if (response == null)
            {
                return BadRequest("Error during login! Check form data!");
            }
            return Ok(response);
        }

        private async Task<JWTAuthorizationResponse> GetAuthorizationResponse(string username, string password)
        {
            var identity = await GetIdentity(username, password);
            if (identity == null)
            {
                return null;
            }
            var jwt = new JwtSecurityToken(
                   issuer: AuthorizationHelper.JWT_ISSUER,
                   audience: AuthorizationHelper.JWT_AUDIENCE,
                   notBefore: DateTime.UtcNow,
                   claims: identity.Claims,
                   expires: DateTime.UtcNow.Add(AuthorizationHelper.JWT_LIFETIME),
                   signingCredentials: new SigningCredentials(AuthorizationHelper.GetSymmetricJWTSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return new JWTAuthorizationResponse
            {
                Access_Token = encodedJwt,
                UserName = identity.Name,
                Roles = identity.Claims.Where(s => s.Type == ClaimsIdentity.DefaultRoleClaimType).Select(s => s.Value).ToList()
            };
        }
        private async Task<ClaimsIdentity> GetIdentity(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return null;
            }
            if (!await _userManager.CheckPasswordAsync(user, password))
            {
                return null;
            }
            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                };
            var roles = await _userManager.GetRolesAsync(user);
            if (roles != null)
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role));
                }
            }
            return new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }

        [HttpPost("/register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Form Data!");
            }
            var user = new User { UserName = model.UserName, Email = model.UserName };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest("Error during registration! Try again!");
            }
            var response = await GetAuthorizationResponse(user.UserName, model.Password);
            if (response == null)
            {
                return BadRequest("Error during login! Try Again by route '/token'");
            }
            return Ok(response);
        }
    }
}
