using EventAndMediaHub.Data;
using EventAndMediaHub.Interface;
using EventAndMediaHub.Models;
using Microsoft.EntityFrameworkCore;

namespace EventAndMediaHub.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserDto>> ListUsers()
        {
            var users = await _context.Users.ToListAsync();

            var userDtos = users.Select(user => new UserDto()
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role,
            }).ToList();

            return userDtos;
        }

        public async Task<UserDto> GetUser(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return null;
            }

            return new UserDto()
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role,
            };
        }

        public async Task<ServiceResponse> CreateUser(UserDto userDto)
        {
            var serviceResponse = new ServiceResponse();

            var user = new User()
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                Role = userDto.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            serviceResponse.CreatedId = user.UserId;
            serviceResponse.Messages.Add($"User created successfully: {user.UserName}");

            return serviceResponse;
        }

        public async Task<ServiceResponse> UpdateUserDetails(int id, UserDto userDto)
        {
            var serviceResponse = new ServiceResponse();

            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("User not found.");
                return serviceResponse;
            }

            existingUser.UserName = userDto.UserName ?? existingUser.UserName;
            existingUser.Email = userDto.Email ?? existingUser.Email;
            existingUser.Role = userDto.Role ?? existingUser.Role;

            await _context.SaveChangesAsync();

            serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
            serviceResponse.Messages.Add($"User updated successfully: {existingUser.UserName}");

            return serviceResponse;
        }

        public async Task<ServiceResponse> DeleteUser(int id)
        {
            var serviceResponse = new ServiceResponse();

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("User cannot be deleted because it does not exist.");
                return serviceResponse;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            serviceResponse.Status = ServiceResponse.ServiceStatus.Deleted;
            serviceResponse.Messages.Add("User deleted successfully.");

            return serviceResponse;
        }
    }
}
