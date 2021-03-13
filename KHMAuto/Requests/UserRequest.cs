
using System.ComponentModel.DataAnnotations;

namespace KHMAuto.Requests
{
    public class UserRequest
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

        public bool isAdmin { get; set; }
        
        public UserRequest() { }
    }
}
