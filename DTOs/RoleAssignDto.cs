using System;
using System.Collections.Generic;

namespace MiniIAM.DTOs
{
    public class RoleAssignDto
    {
        public Guid Id { get; set; } = Guid.NewGuid(); // Unique identifier for the role

    }
}
