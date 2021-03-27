
using System.ComponentModel.DataAnnotations;

namespace KHMAuto.Requests
{
    public class UserRegisterRequest
    {
        public int UserId { get; set; }
        [Required]
        [StringLength(24, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [StringLength(24, MinimumLength = 3)]
        public string Password { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public bool IsAdmin { get; set; }

        public string NewPassword { get; set; }

        public string RegisterSecret { get; set; }

        public UserRegisterRequest() { }
    }
}
