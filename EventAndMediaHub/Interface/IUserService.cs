using EventAndMediaHub.Models;

namespace EventAndMediaHub.Interface
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> ListUsers();
        Task<UserDto> GetUser(int id);
        Task<ServiceResponse> CreateUser(UserDto userDto);
        Task<ServiceResponse> UpdateUserDetails(int id, UserDto userDto);
        Task<ServiceResponse> DeleteUser(int id);
    }
}
