using AutoMapper;
using Data.Domain.Models;
using Data.Domain.Repositories;
using Data.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public static string HashPassword(string password)
        {
            return password;
        }

        public static bool IsPasswordMatch(string password, string hashPassword)
        {
            return HashPassword(password).Equals(hashPassword);
        }

        public UserService(IUserRepository repository, IMapper mapper, UserManager<User> userManager,SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _repository = repository;
            _mapper = mapper;

        }

        public async Task Register(UserDto user)
        {
            if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password) || string.IsNullOrWhiteSpace(user.FullName))
            {
                throw new ArgumentException("User information is required");
            }
            var existedUser = await _userManager.FindByNameAsync(user.Username);
            if (existedUser != null)
            {
                throw new ArgumentException("Username is already existed");
            }
            var newUser = new User()
            {
                UserName = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                isAdmin = user.isAdmin
            };
            var result = await _userManager.CreateAsync(newUser, user.Password);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException("Cannot create user" + string.Join(",", result.Errors));
            }
        }

        public async Task<UserDto> SignIn(UserDto user)
        {
            var checkUser = await _userManager.FindByNameAsync(user.Username);
            if (checkUser != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(checkUser, user.Password, false);
                if (result.Succeeded)
                {
                    return _mapper.Map<UserDto>(checkUser);
                }
            }
            return null;
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
            //updateUser.Username = user.Username;
            updateUser.FullName = user.FullName;
            
            if (!string.IsNullOrWhiteSpace(user.Password))
            {
                //updateUser.Password = HashPassword(user.Password);
            }
            await _repository.UpdateUser(updateUser);
        }

        public async Task<UserDto> GetByUsername(string username)
        {
            //var user = await _repository.GetByUsername(username);
            //var user = await _userManager.Users.FirstOrDefaultAsync(user => user.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return null;
            return _mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> GetAll()
        {
            var users = await _userManager.Users.ToListAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> GetById(int id)
        {
            //var user = await _repository.GetById(id);
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return null;
            return _mapper.Map<UserDto>(user);
        }
    }
}
