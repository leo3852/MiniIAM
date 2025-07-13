// Archivo: Data/DbInitializer.cs
using MiniIAM.Models;
using System.Collections.Generic;
using System.Linq;

namespace MiniIAM.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            //verify if the database has been seeded
            if (context.Users.Any())
                return;

            //new roles
            var roles = new List<Role>
            {
                new Role { RoleName = "Admin" },
                new Role { RoleName = "User" }
            };

            context.Roles.AddRange(roles);

            //new users
            var users = new List<User>
            {
                new User
                {
                    Name = "TestUser",
                    Email = "testuser@example.com",
                    Password = "123456",
                    Roles = new List<Role> { roles[1] } // with rol "User"
                },
                new User
                {
                    Name = "AdminUser",
                    Email = "admin@example.com",
                    Password = "admin123",
                    Roles = new List<Role> { roles[0] } // with rol "Admin"
                }
            };

            context.Users.AddRange(users);

            context.SaveChanges();
        }
    }
}
