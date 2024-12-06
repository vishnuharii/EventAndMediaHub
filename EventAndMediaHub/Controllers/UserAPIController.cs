using EventAndMediaHub.Interface;
using EventAndMediaHub.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventAndMediaHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAPIController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserAPIController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("List")]
        public async Task<IEnumerable<UserDto>> ListUsers()
        {
            return await _userService.ListUsers();
        }

        [HttpGet("{id}")]
        public async Task<UserDto> GetUser(int id)
        {
            return await _userService.GetUser(id);
        }

        [HttpPost("Add")]
        public async Task<ServiceResponse> CreateUser(UserDto userDto)
        {
            return await _userService.CreateUser(userDto);
        }

        [HttpPut("Update/{id}")]
        public async Task<ServiceResponse> UpdateUserDetails(int id, UserDto userDto)
        {
            return await _userService.UpdateUserDetails(id, userDto);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ServiceResponse> DeleteUser(int id)
        {
            return await _userService.DeleteUser(id);
        }
    }
}
