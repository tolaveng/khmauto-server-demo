using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Data.Domain.Models
{
    public class User: IdentityUser<int>
    {
        public string FullName { get; set; }
        public bool isAdmin { get; set; }

        public RefreshToken RefreshToken { get; set; } = new RefreshToken();

    }
}
