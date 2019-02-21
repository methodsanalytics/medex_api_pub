﻿using Medical_Examiner_API.Loggers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Okta.AspNetCore;

namespace Medical_Examiner_API.Controllers
{
    /// <summary>
    /// Accounts controller, handler for authentication and token verification
    /// </summary>
    [Route("auth")]
    [ApiController]
    public class AccountController : BaseController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="logger">
        /// initialise with IMELogger instance
        /// </param>
        public AccountController(IMELogger logger)
            : base(logger)
        {
        }

        /// <summary>
        /// authenticates an authorised token for use with the data API
        /// </summary>
        /// <returns></returns>
        [HttpGet("login")]
        public IActionResult Login()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Challenge(OktaDefaults.MvcAuthenticationScheme);
            }

            return Unauthorized();
        }

        /// <summary>
        /// calls to Okta and Logs out the current authenticated user token
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Logout()
        {
            return new SignOutResult(new[]
                {OktaDefaults.MvcAuthenticationScheme, CookieAuthenticationDefaults.AuthenticationScheme});
        }
    }
}