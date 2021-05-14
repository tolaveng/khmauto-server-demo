using Data.Domain.Models;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Api.Services
{
    public interface IUserService
    {
        public Task<UserDto> GetByUsername(string username);
        public Task<IEnumerable<UserDto>> GetAll();
        public Task<UserDto> GetById(int id);
        public Task Register(UserDto user);
        public Task<UserDto> SignIn(UserDto user);
        public Task UpdateUser(UserDto user, string newPassword = null);
        public Task<RefreshToken> GetRefreshTokenByUsername(string username, string refreshToken);
        public Task<UserDto> CreateNewToken(UserDto user);
    }
}
