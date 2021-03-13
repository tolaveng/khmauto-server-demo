using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Data.Api.Services;
using Data.DTO;
using KHMAuto.Requests;
using KHMAuto.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KHMAuto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        // api/user/register
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] UserRequest user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
            {
                return BadRequest();
            }
            var newUser = new UserDto() {
                FullName = user.FullName,
                Password = user.Password,
                Username = user.Username,
                Email = user.Email,
                isAdmin = true
            };
            try
            {
                await _userService.Register(newUser);
            }catch(Exception ex)
            {
                return Json(ResponseResult<string>.Fail(ex.Message));
            }
            var response = ResponseResult<bool>.Success("", true);
            return Json(response);
        }

        [HttpPost("update")]
        public async Task<ActionResult<ResponseResult<string>>> Update([FromBody] UserRequest user)
        {
            var updateUser = new UserDto()
            {
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                Password = user.Password,
            };
            try
            {
                await _userService.UpdateUser(updateUser, user.NewPassword);
                return Json(ResponseResult<bool>.Success("", true));

            } catch (Exception ex)
            {
                return Json(ResponseResult<string>.Fail("Update failed", ex.Message));
            }
            return Json(ResponseResult<string>.Fail("Update failed"));
        }


        [HttpGet("getusers")]
        public async Task<JsonResult> GetUsers()
        {
            var users = await _userService.GetAll();
            var results = users.Select(z => new UserRequest() {
                UserId = z.UserId,
                FullName = z.FullName,
                Username = z.Username,
                isAdmin = z.isAdmin
            }).ToArray();
            return Json(results);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _userService.GetById(id);
            if (user != null)
            {
                user.Password = string.Empty;
                var response = ResponseResult<UserDto>.Success("Get user successful", user);
                return Json(response);
            }
            return Json(ResponseResult<string>.Fail("Get user failed"));
        }


        [HttpPost("login")]
        public async Task<ActionResult<ResponseResult<string>>> Login([FromBody] UserRequest user)
        {
            var signInUser = await _userService.SignIn(new UserDto() { 
                Username = user.Username,
                Password = user.Password
            });
            if (signInUser != null)
            {
                var response = ResponseResult<UserDto>.Success("Sign in successful", signInUser);
                return Json(response);
            }
            
            return Json(ResponseResult<string>.Fail("Login failed"));
        }
    }
}
