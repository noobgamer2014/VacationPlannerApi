using Microsoft.AspNetCore.Mvc;
using VacationPlanner.Models;
using VacationPlanner.Services;
using System.Threading.Tasks;

namespace VacationPlanner.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthenticationController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            // Generate JWT token, map to DTO, etc., and return as part of the response

            return Ok(new
            {
                Id = user.Id,
                Username = user.Name,
                Email = user.Email,
                Token = "fake-jwt-token" // You should replace this with an actual JWT token
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            try
            {
                // Create a new user
                var user = await _userService.Register(model.Username, model.Email, model.Password);
                return Ok(new
                {
                    Id = user.Id,
                    Username = user.Name,
                    Email = user.Email,
                    // Do not return the password or its hash
                });
            }
            catch (AppException ex)
            {
                // Return an error response if registration fails
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    public class RegisterModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
