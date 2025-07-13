using Xunit;
using Moq;
using System.Threading.Tasks;
using MiniIAM.Services;
using MiniIAM.Repositories;
using MiniIAM.Models;
using MiniIAM.DTOs;

namespace MiniIAM.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IRoleRepository> _roleRepositoryMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _roleRepositoryMock = new Mock<IRoleRepository>();
            _userService = new UserService(_userRepositoryMock.Object, _roleRepositoryMock.Object);
        }

        [Fact]
        public async Task SimulateLoginAsync_ValidCredentials_ReturnsTrue()
        {
            // Arrange
            var dto = new LoginDto
            {
                Email = "test@example.com",
                Password = "123456"
            };

            _userRepositoryMock
                .Setup(repo => repo.GetByEmailAndPasswordAsync(dto.Email, dto.Password))
                .ReturnsAsync(new User { Id = Guid.NewGuid(), Email = dto.Email, Password = dto.Password });

            // Act
            var result = await _userService.SimulateLoginAsync(dto);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task SimulateLoginAsync_InvalidCredentials_ReturnsFalse()
        {
            // Arrange
            var dto = new LoginDto
            {
                Email = "wrong@example.com",
                Password = "wrongpass"
            };

            _userRepositoryMock
                .Setup(repo => repo.GetByEmailAndPasswordAsync(dto.Email, dto.Password))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _userService.SimulateLoginAsync(dto);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task CreateUserAsync_ValidUser_CallsRepositoryAndReturnsUser()
        {
            var dto = new UserDto
            {
                Name = "Leonardo",
                Email = "leo@example.com",
                Password = "abcdef"
            };

            var expectedUser = new User
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
                Password = dto.Password
            };

            _userRepositoryMock
                .Setup(repo => repo.CreateAsync(It.IsAny<User>()))
                .ReturnsAsync(expectedUser);

            var result = await _userService.CreateUserAsync(dto);

            Assert.NotNull(result);
            Assert.Equal(dto.Email, result.Email);
            _userRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task AssignRoleAsync_UserExists_AssignsRole()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var roleId = Guid.NewGuid();
            var roleDto = new RoleAssignDto { Id = roleId };

            var user = new User
            {
                Id = userId,
                Name = "Leonardo",
                Email = "test@example.com",
                Password = "123456",
                Roles = new List<Role>()
            };

            var role = new Role
            {
                Id = roleId,
                RoleName = "Admin"
            };

            _userRepositoryMock
                .Setup(repo => repo.GetByIdAsync(userId))
                .ReturnsAsync(user);

            _roleRepositoryMock
                .Setup(repo => repo.GetByIdAsync(roleId))
                .ReturnsAsync(role);

            _userRepositoryMock
                .Setup(repo => repo.UpdateAsync(user))
                .Returns(Task.CompletedTask);

            // Act
            await _userService.AssignRoleAsync(userId, roleDto.Id);

            // Assert
            Assert.Contains(user.Roles, r => r.Id == roleId && r.RoleName == "Admin");
            _userRepositoryMock.Verify(repo => repo.UpdateAsync(user), Times.Once);
        }

        [Fact]
        public async Task GetUserByIdAsync_UserExists_ReturnsUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, Name = "Leonardo", Email = "leo@example.com" };

            _userRepositoryMock
                .Setup(repo => repo.GetByIdAsync(userId))
                .ReturnsAsync(user);

            // Act
            var result = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
        }

        [Fact]
        public async Task CreateUserAsync_InvalidEmail_ThrowsArgumentException()
        {
            var dto = new UserDto
            {
                Name = "Leonardo",
                Email = "invalid-email",
                Password = "abcdef"
            };

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => _userService.CreateUserAsync(dto));
            Assert.Equal("Invalid email format.", ex.Message);
        }

        [Fact]
        public async Task CreateUserAsync_ShortPassword_ThrowsArgumentException()
        {
            var dto = new UserDto
            {
                Name = "Leonardo",
                Email = "leo@example.com",
                Password = "123"
            };

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => _userService.CreateUserAsync(dto));
            Assert.Equal("Password must be at least 6 characters.", ex.Message);
        }

        [Fact]
        public async Task CreateUserAsync_EmptyName_ThrowsArgumentException()
        {
            var dto = new UserDto
            {
                Name = "",
                Email = "leo@example.com",
                Password = "abcdef"
            };

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => _userService.CreateUserAsync(dto));
            Assert.Equal("Name must be at least 6 characters.", ex.Message);
        }


        [Fact]
        public async Task GetUserWithRolesAsync_UserExists_ReturnsUserWithRoles()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                Name = "Test User",
                Email = "test@example.com",
                Password = "123456",
                Roles = new List<Role>
                {
                    new Role { RoleName = "Admin" },
                    new Role { RoleName = "User" }
                }
            };

            // Mock the repository to return the user with roles
            _userRepositoryMock
                .Setup(r => r.GetByIdAsync(userId))
                .ReturnsAsync(user);

            // Act
            var result = await _userService.GetUserWithRolesAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test User", result.Name);
            Assert.Equal("test@example.com", result.Email);
            Assert.NotNull(result.Roles);
            Assert.Equal(2, result.Roles.Count);
            Assert.Contains(result.Roles, r => r.RoleName == "Admin");
            Assert.Contains(result.Roles, r => r.RoleName == "User");
        }

        [Fact]
        public async Task GetUserWithRolesAsync_UserDoesNotExist_ReturnsNull()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _userRepositoryMock
                .Setup(r => r.GetByIdAsync(userId))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _userService.GetUserWithRolesAsync(userId);

            // Assert
            Assert.Null(result);
        }


    }
}
