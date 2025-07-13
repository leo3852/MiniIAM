using Microsoft.AspNetCore.Mvc;
using MiniIAM.DTOs;
using MiniIAM.Services;

namespace MiniIAM.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        // [POST] /login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var success = await _userService.SimulateLoginAsync(dto);

            if (!success)
                return Unauthorized("Invalid credentials.");

            return Ok("Login successful.");
        }
    }
}
