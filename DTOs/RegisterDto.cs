using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IndieForge.Models;

namespace IndieForge.DTOs
{
    public class RegisterDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}