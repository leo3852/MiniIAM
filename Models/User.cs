using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MiniIAM.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid(); // Unique identifier for the user

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; }

        // Lista de roles asignados a este usuario
        public List<Role> Roles { get; set; } = new();
    }
}
