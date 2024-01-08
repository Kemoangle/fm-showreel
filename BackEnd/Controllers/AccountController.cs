using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Models.ViewModels;
using BackEnd.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;
        public AccountController(IAccountService _accountService)
        {
            accountService = _accountService;
        }
        [AllowAnonymous]
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpModel signUpModel)
        {
            var response = await accountService.SignUpAsync(signUpModel);
            if (response.Succeeded)
            {
                return Ok(response.Succeeded);
            }
            return Unauthorized();

        }

        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignInModel signInModel)
        {
            var response = await accountService.SignInAsync(signInModel);
            if (string.IsNullOrEmpty(response))
            {
                return Unauthorized();
            }
            return Ok(response);

        }

        [HttpGet]
        public IActionResult Protected()
        {
            return Ok("Protected area");
        }
    }
}