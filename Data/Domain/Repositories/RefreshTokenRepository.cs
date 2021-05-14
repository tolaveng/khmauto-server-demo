using Data.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDataContext context;
        public RefreshTokenRepository(AppDataContext context)
        {
            this.context = context;
        }
        public async Task RemoveAll(int userId)
        {
            var refreshTokens = await context.RefreshTokens.Where(x => x.UserId == userId).ToArrayAsync();
            if (!refreshTokens.Any()) return;

            context.RefreshTokens.RemoveRange(refreshTokens);
            await context.SaveChangesAsync();
        }
    }
}
