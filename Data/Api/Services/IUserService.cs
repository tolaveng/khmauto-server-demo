using Data.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Api.Services
{
    public interface IUserService
    {
        public Task AddUser(UserDto user);
        public Task UpdateUser(UserDto user);
    }
}
