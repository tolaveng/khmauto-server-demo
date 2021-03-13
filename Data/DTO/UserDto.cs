using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DTO
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool isAdmin { get; set; }

        public string JwtToken { get; set; }

        public UserDto() { }
    }
}
