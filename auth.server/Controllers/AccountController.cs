using System;
using System.Threading.Tasks;
using CodeWorks.Auth0Provider;
using auth.server.Models;
using auth.server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace auth.server.Controllers
{
    [ApiController]
    [Route("[controller]")]

    [Authorize]
    // STUB[epic=Auth] Adds Authguard to all routes on the whole controller

    public class AccountController : ControllerBase
    {
        // NOTE readonly means the property can only be set in the constructor and cannot be modified outside of it
        private readonly AccountsService _service;

        public AccountController(AccountsService service)
        {
            _service = service;
        }

        [HttpGet]

        public async Task<ActionResult<Account>> Get()
        // NOTE asyncronous actions must include "Task" before ActionResult
        {
            // STUB[epic=Auth] Replaces req.userinfo
            // NOTE HOW TO GET ACTIVE USERS
            try
            {
                Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
                Account currentUser = _service.GetOrCreateAccount(userInfo);
                return Ok(currentUser);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }
    }
}