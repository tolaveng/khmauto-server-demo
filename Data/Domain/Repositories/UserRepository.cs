using Data.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDataContext context;
        public UserRepository(AppDataContext context)
        {
            this.context = context;
        }

        public async Task<User> AddUser(User user)
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetById(int userId)
        {
            return await context.Users.FirstOrDefaultAsync(z => z.UserId == userId);
        }

        public async Task<User> GetByUsername(string userName)
        {
            return await context.Users.FirstOrDefaultAsync(z => z.Username.Equals(userName, StringComparison.OrdinalIgnoreCase));
        }

        public async Task UpdateUser(User user)
        {
            var change = context.Users.Attach(user);

            if (string.IsNullOrEmpty(user.Password))
            {
                context.Entry(user).Property(z => z.Password).IsModified = false;
            }

            change.State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
