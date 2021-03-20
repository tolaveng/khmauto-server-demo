using AutoMapper;
using Data.Domain.Models;
using Data.DTO;
using Data.Interfaces;
using Data.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Api.Services
{
    public class UserService : IUserService
    {
 
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtGenerator _jwtGenerator;

        public UserService(IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, IJwtGenerator jwtGenerator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtGenerator = jwtGenerator;
            _mapper = mapper;

        }


        public async Task Register(UserDto user)
        {
            if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password) || string.IsNullOrWhiteSpace(user.FullName))
            {
                throw new ArgumentException("User information is required");
            }
            var existedUser = await _userManager.FindByNameAsync(user.Username.CleanText());
            if (existedUser != null)
            {
                throw new ArgumentException("Username is already existed");
            }
            var newUser = new User()
            {
                UserName = user.Username.CleanText(),
                FullName = user.FullName,
                Email = user.Email.CleanText(),
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
                    var userDto = _mapper.Map<UserDto>(checkUser);
                    userDto.JwtToken = _jwtGenerator.CreateToken(checkUser);
                    return userDto;
                }
            }
            return null;
        }

        
        public async Task UpdateUser(UserDto user, string newPassword = null)
        {
            if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.FullName) || string.IsNullOrWhiteSpace(user.Password))
            {
                throw new ArgumentException("User information is required");
            }
            var checkUser = await _userManager.FindByNameAsync(user.Username);
            if (checkUser == null)
            {
                throw new InvalidOperationException("User is not found");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(checkUser, user.Password, false);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException("User password is not matched");
            }

            checkUser.FullName = user.FullName;
            checkUser.Email = user.Email;

            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                checkUser.PasswordHash = _userManager.PasswordHasher.HashPassword(checkUser, newPassword);
            }
            await _userManager.UpdateAsync(checkUser);
        }


        public async Task<UserDto> GetByUsername(string username)
        {
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
