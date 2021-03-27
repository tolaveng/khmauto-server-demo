using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Data.Api.Services;
using Data.DTO;
using KHMAuto.Requests;
using KHMAuto.Responses;
using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] UserRegisterRequest user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.Username) ||
                string.IsNullOrWhiteSpace(user.Password) ||
                user.RegisterSecret != "T0LA"
                )
            {
                return BadRequest();
            }
            var newUser = new UserDto() {
                FullName = user.FullName,
                Password = user.Password,
                Username = user.Username,
                Email = user.Email,
                isAdmin = user.IsAdmin
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


        [Authorize]
        [HttpPost("update")]
        public async Task<ActionResult> Update([FromBody] UserRegisterRequest user)
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
        }

        [Authorize]
        [HttpGet("getusers")]
        public async Task<JsonResult> GetUsers()
        {
            var users = await _userService.GetAll();
            return Json(users);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetUser(int id)
        {
            var user = await _userService.GetById(id);
            if (user != null)
            {
                user.Password = string.Empty;
                var response = ResponseResult<UserDto>.Success("Get user successful", user);
                return Ok(response);
            }
            return BadRequest(ResponseResult<string>.Fail("Get user failed"));
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserRegisterRequest user)
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
            
            return BadRequest(ResponseResult<string>.Fail("Login failed"));
        }

        [Authorize]
        [HttpGet("currentuser")]
        public async Task<ActionResult> CurrentUser()
        {
            var username = HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(username)) return Unauthorized();

            var user = await _userService.GetByUsername(username);
            if (user != null)
            {
                return Json(user);
            }
            return Unauthorized();
        }
    }
}
