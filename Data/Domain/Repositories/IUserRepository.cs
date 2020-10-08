using Data.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Repositories
{
    public interface IUserRepository
    {

        public Task<User> AddUser(User user);
        public Task<IEnumerable<User>> GetAll();
        public Task<User> GetById(int userId);
        public Task<User> GetByUsername(string userName);
        public Task UpdateUser(User user);
    }
}
