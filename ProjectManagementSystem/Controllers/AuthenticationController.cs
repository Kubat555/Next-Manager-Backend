
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Services.Interfaces;
using ProjectManagement.Services.Models;
using ProjectManagement.Services.Models.Authentication.Login;
using ProjectManagement.Services.Models.Authentication.Signup;

namespace ProjectManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;

        public AuthenticationController
            (
                IEmailService emailService,
                IUserService userService)
        {
            _emailService = emailService;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUser registerUser)
        {
            var result = await _userService.CreateUserWithTokenAsync(registerUser);
            if (result.isSuccess)
            {
                var confirmationLink = Url.Action(nameof(ConfirmEmail), "Authentication", new { token = result.Response, email = registerUser.Email }, Request.Scheme);

                var content = _emailService.GetHtmlConfirmEmail(registerUser.FirstName!, confirmationLink!);

                var message = new Message(new string[] { registerUser.Email! }, "Confirmation email link", content!);
                _emailService.SendEmail(message);

                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            //checking the user
            var result = await _userService.LoginUserWithJwtTokenAsync(loginModel);

            if (result.isSuccess)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = false,
                    Expires = DateTime.UtcNow.AddHours(3),
                    SameSite = SameSiteMode.Strict,
                    Secure = true,
                    Path ="/"
                };

                Response.Cookies.Append("token", result.Response.Token, cookieOptions);
                return Ok(result);
            }
            return Unauthorized(result);
        }


        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var confirm = await _userService.ConfirmUserEmailAsync(token, email);
            if (confirm.isSuccess)
            {
                return Redirect("https://next-manager-iota.vercel.app/auth/register/email/confirm");
            }

            return StatusCode(confirm.StatusCode, confirm.Message);
        }

    }
}
