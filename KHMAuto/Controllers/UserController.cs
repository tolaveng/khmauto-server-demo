using System;
using System.Collections.Generic;
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

        // api/user/createuser
        [HttpPost("createuser")]
        public async Task<ActionResult> CreateUser([FromBody] UserRequest user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
            {
                return BadRequest();
            }
            var newUser = new UserDto() {
                FullName = user.FullName,
                Password = user.Password,
                Username = user.Username,
                isAdmin = true
            };
            try
            {
                await _userService.AddUser(newUser);
            }catch(Exception ex)
            {
                return Json(ResultResponse.Fail(ex.Message));
            }
            return Ok();
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
            if (user == null)
            {
                return null;
            }
            user.Password = string.Empty;
            return user;
        }
    }
}
