using Data.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Interfaces
{
    public interface IJwtGenerator
    {
        string CreateToken(User user);

        public RefreshToken GenerateRefreshToken();
    }
}
