using System;
using System.Threading.Tasks;
using MiniIAM.Models;
using MiniIAM.DTOs;
using MiniIAM.Repositories;

namespace MiniIAM.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<User> CreateUserAsync(UserDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name) || dto.Name.Length < 6)
                throw new ArgumentException("Name must be at least 6 characters.");

            if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 6)
                throw new ArgumentException("Password must be at least 6 characters.");

            if (!IsValidEmail(dto.Email))
                throw new ArgumentException("Invalid email format.");

            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
                throw new InvalidOperationException("User with this email already exists.");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
                Password = dto.Password // in a real app, hash the password before saving
            };

            await _userRepository.CreateAsync(user);
            return user;
        }

        public async Task AssignRoleAsync(Guid userId, Guid roleId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            var role = await _roleRepository.GetByIdAsync(roleId);
            if (role == null)
                throw new KeyNotFoundException("Role not found.");

            // Check if the user already has this role
            if (user.Roles.Any(r => r.Id == roleId))
                throw new ArgumentException("User already has this role.");

            user.Roles.Add(role);
            await _userRepository.UpdateAsync(user);
        }



        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<bool> SimulateLoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAndPasswordAsync(dto.Email, dto.Password);
            return user != null;
        }

        private bool IsValidEmail(string email)
        {
            return System.Net.Mail.MailAddress.TryCreate(email, out _);
        }

        public async Task<UserDto?> GetUserWithRolesAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;

            return new UserDto
            {
                Name = user.Name,
                Email = user.Email,
                Roles = user.Roles.Select(r => new RoleDto
                {
                    RoleName = r.RoleName
                }).ToList()
            };
        }

    }

    public interface IUserService
    {
        Task<User> CreateUserAsync(UserDto user);
        Task<User?> GetUserByIdAsync(Guid id);

        Task<UserDto?> GetUserWithRolesAsync(Guid userId);

        Task AssignRoleAsync(Guid userId, Guid roleId);


        // simulate login for demonstration purposes
        Task<bool> SimulateLoginAsync(LoginDto dto);

    }
    
}
