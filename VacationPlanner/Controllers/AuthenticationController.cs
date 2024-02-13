using Microsoft.AspNetCore.Mvc;
using VacationPlanner.Models;
using VacationPlanner.Services;

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
        public IActionResult Login([FromBody] LoginModel model)
        {
            var user = _userService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            // Convert User to UserDTO
            var userDto = new UserDTO
            {
                //Name = user.Username,
                //Password = user.Password,
                // Map other required properties here.
            };

            return Ok(userDto);
        }
        //[HttpPost("register")]
        
    }



}
