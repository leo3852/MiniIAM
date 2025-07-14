using Microsoft.AspNetCore.Mvc;
using MiniIAM.Repositories;

namespace MiniIAM.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;

        public RolesController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        [HttpGet]
        public IActionResult GetRoles()
        {
            var roles = _roleRepository.GetAllAsync();
            return Ok(roles);
        }
    }
}
