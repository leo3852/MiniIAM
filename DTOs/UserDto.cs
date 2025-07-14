using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace MiniIAM.DTOs
{
    public class UserDto
    {

        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public List<RoleDto> Roles { get; set; } = new List<RoleDto>();
    }
}