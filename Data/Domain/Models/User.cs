using Microsoft.AspNetCore.Identity;

namespace Data.Domain.Models
{
    public class User: IdentityUser<int>
    {
        public string FullName { get; set; }
        public bool isAdmin { get; set; }

    }
}
