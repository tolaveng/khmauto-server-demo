using AutoMapper;
using Data.Domain.Models;
using Data.Domain.Repositories;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public static string HashPassword(string password)
        {
            return password;
        }

        public static bool IsPasswordMatch(string password, string hashPassword)
        {
            return HashPassword(password).Equals(hashPassword);
        }

        public UserService(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;

        }

        public async Task AddUser(UserDto user)
        {
            if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password) || string.IsNullOrWhiteSpace(user.FullName))
            {
                throw new ArgumentException("User information is required");
            }
            var newUser = _mapper.Map<User>(user);
            newUser.Password = HashPassword(user.Password);
            newUser.isAdmin = true;
            await _repository.AddUser(newUser);
        }

        public async Task UpdateUser(UserDto user)
        {
            if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.FullName))
            {
                throw new ArgumentException("User information is required");
            }
            var updateUser = await _repository.GetByUsername(user.Username);
            if (updateUser == null)
            {
                throw new InvalidOperationException("User is not found");
            }
            updateUser.Username = user.Username;
            updateUser.FullName = user.FullName;
            
            if (!string.IsNullOrWhiteSpace(user.Password))
            {
                updateUser.Password = HashPassword(user.Password);
            }
            await _repository.UpdateUser(updateUser);
        }

    }
}
