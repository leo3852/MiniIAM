using MiniIAM.Models;
using MiniIAM.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniIAM.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<List<Role>> GetAllAsync()
        {
            return await _roleRepository.GetAllAsync();
        }
    }

    public interface IRoleService
    {
        Task<List<Role>> GetAllAsync();
    }
}
