using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User user { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; } = DateTime.UtcNow.AddDays(1);
        public bool IsExpired => DateTime.UtcNow >= Expires;
    }
}
