using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.Enums;
using FestivalZnanostiApi.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace FestivalZnanostiApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {


        private readonly IAuthService _authService;



        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {

            var accountId = await this._authService.registerAccount(registerDto);

            var response = new Response
            {
                Message = "Račun je uspješno registriran"
            };

            return Ok(response);

        }



        // POST api/<AuthenticationController>
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {

            var account = await this._authService.loginAccount(loginDto);
            var response = new Response();

            var userRole = (account.Role == (int)UserRole.Administrator) ? "Administrator" : "Submitter";

            var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, Convert.ToString(account.Id)),
                    new Claim(ClaimTypes.Email,account.Email),
                    new Claim(ClaimTypes.Role,  userRole),
                    // Any additional custom claim --> new Claim("DotNetMania", "Code")

                };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
            {
                IsPersistent = true
            });

            response.Message = "Prijava uspješna";
            return Ok(response);
        }


        [HttpPost]
        [Route("Logout")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var response = new Response
            {
                Message = "Odjava uspješna"
            };

            return Ok(response);
        }



        /*[Authorize]
        [HttpGet]
        [Route("restricted")]
        public string Restricted()
        {
            return "Dobar si";
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("admin")]
        public string Admin()
        {
            return "Dobar si admin";
        }
        */

    }
}
