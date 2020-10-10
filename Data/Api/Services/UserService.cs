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
            var existedUser = await GetByUsername(user.Username);
            if (existedUser != null)
            {
                throw new ArgumentException("User name is already existed");
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

        public async Task<UserDto> GetByUsername(string username)
        {
            var user = await _repository.GetByUsername(username);
            if (user == null) return null;
            return _mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> GetAll()
        {
            var users = await _repository.GetAll();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> GetById(int id)
        {
            var user = await _repository.GetById(id);
            if (user == null) return null;
            return _mapper.Map<UserDto>(user);
        }
    }
}
