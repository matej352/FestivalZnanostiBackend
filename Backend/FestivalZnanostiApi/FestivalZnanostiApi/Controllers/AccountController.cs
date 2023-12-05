using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.Middlewares.UserContext;
using FestivalZnanostiApi.Services;
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

        //ovo ostalo samo ovak stoji

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
    }
}
