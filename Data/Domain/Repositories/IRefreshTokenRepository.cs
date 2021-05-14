using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task RemoveAll(int userId);
    }
}
