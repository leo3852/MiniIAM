using Microsoft.AspNetCore.Mvc;
using MiniIAM.DTOs;
using MiniIAM.Services;
using MiniIAM.Models;

namespace MiniIAM.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // [POST] /users
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDto dto)
        {
            try
            {
                var user = await _userService.CreateUserAsync(dto);
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        // [POST] /users/{id}/roles
        [HttpPost("{id}/roles")]
        public async Task<IActionResult> AssignRole(Guid id, [FromBody] RoleAssignDto dto)
        {
            try
            {
                await _userService.AssignRoleAsync(id, dto.Id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // [GET] /users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound("User not found.");

            return Ok(new
            {
                user.Id,
                user.Name,
                user.Email,
                Roles = user.Roles.Select(r => r.RoleName)
            });
        }
    }
}
