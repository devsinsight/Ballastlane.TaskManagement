using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ballastlane.TaskManagement.Web.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthenticationController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Login successful" });
            }

            return BadRequest(new { Message = "Invalid login attempt" });
        }


    }

    public class LoginModel
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
