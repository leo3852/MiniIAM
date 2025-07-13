using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MiniIAM.Models
{
    public class Role
    {
        public Guid Id { get; set; } = Guid.NewGuid(); // Unique identifier for the role

        [Required(ErrorMessage = "Role name is required")]
        public string RoleName { get; set; }

    }
}
