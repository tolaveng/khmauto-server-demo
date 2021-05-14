using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KHMAuto.Requests
{
    public class RefreshTokenRequest
    {
        public string username { get; set; }
        public string refreshToken { get; set; }
    }
}
