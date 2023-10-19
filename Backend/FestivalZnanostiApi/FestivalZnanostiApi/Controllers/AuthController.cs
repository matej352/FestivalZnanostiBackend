using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace FestivalZnanostiApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        public List<LoginDto> loginDtos = null;

        public AuthController()
        {
            loginDtos = new List<LoginDto>
            {
                new LoginDto()
                {
                    Email = "pero@gmail.com",
                    Password = "password",
                },
                new LoginDto()
                {
                    Email = "ante@gmail.com",
                    Password = "password",
                }
            };
        }


        // POST api/<AuthenticationController>
        [HttpPost]
        [Route("Login")]
        public async Task<string> Login(LoginDto loginDto)
        {
            var user = loginDtos.Where(u => u.Email == loginDto.Email && u.Password == loginDto.Password).FirstOrDefault();
            if (user != null)
            {
                var roleId = (user.Email == "pero@gmail.com") ? 0 : 1;

                var userRole = (roleId == Convert.ToInt32(UserRole.Administrator)) ? "Administrator" : "Submitter";

                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.Email)),
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(ClaimTypes.Role,  userRole),
                    // Any additional custom clam --> new Claim("DotNetMania", "Code")

                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                {
                    IsPersistent = true
                });
                return "Logged in successfully";
            }
            else
            {
                return "Invalid email or password";
            }
        }

        // POST api/<AuthenticationController>
        [HttpGet]
        [Route("logout")]
        public async Task<string> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return "Logged out successfully";
        }



        [Authorize]
        [HttpGet]
        [Route("restricted")]
        public string Restricted()
        {
            var cookieValue = HttpContext.Request.Cookies["FestivalZnanostiCookie"];
            var a = HttpContext.User.Identity;
            if (cookieValue != null)
            {
                Console.WriteLine(cookieValue.ToString());

            }
            return "Dobar si";
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("admin")]
        public string Admin()
        {
            return "Dobar si admin";
        }


    }
}
