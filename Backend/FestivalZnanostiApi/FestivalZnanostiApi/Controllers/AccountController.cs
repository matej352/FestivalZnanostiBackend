using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.Enums;
using FestivalZnanostiApi.Middlewares.UserContext;
using FestivalZnanostiApi.Services;
using FestivalZnanostiApi.Services.impl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FestivalZnanostiApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserContext _userContext;
        private readonly IAccountService _accountService;

        public AccountController(
            IAccountService accountService,
            UserContext userContext)
        {
            _accountService = accountService;
            _userContext = userContext;
        }


        [Authorize]
        [HttpGet]
        [Route("Details")]
        public async Task<ActionResult<AccountDto>> GetAccountDetails()
        {
            var details = await _accountService.GetAccount(_userContext.Id);
            return Ok(details);
        }

        [Authorize]
        [HttpPut]
        [Route("ChangePassword/{id}")]
        public async Task<ActionResult> ChangePassword(int id, ChangePasswordDto changePasswordDto)
        {
            if (id != changePasswordDto.AccountId)
            {
                return BadRequest("Account id mismatch!");
            }


            if (_userContext.Role == UserRole.Administrator)
            {

                //case: Admin changes his own password
                if (id == _userContext.Id)
                {
                    await _accountService.ChangeMyPassword(changePasswordDto);
                }

                await _accountService.UserForgotPassword(changePasswordDto.AccountId, changePasswordDto.NewPassword, changePasswordDto.ConfirmNewPassword);
            }

            else if (_userContext.Role == UserRole.Submitter)
            {

                if (_userContext.Id != changePasswordDto.AccountId)
                {
                    return Forbid("You can change only your password!");
                }

                await _accountService.ChangeMyPassword(changePasswordDto);

            }

            return Ok("Lozinka uspješno promijenjena.");

        }



        //ovo ostalo samo ovak stoji
        /*
        // GET api/<AccountController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AccountController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AccountController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AccountController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        */
    }
}
