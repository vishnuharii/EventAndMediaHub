using EventAndMediaHub.Models;
using Microsoft.AspNetCore.Http;

namespace EventAndMediaHub.Interface
{
    public interface IPhotoService
    {
        Task<IEnumerable<PhotoDto>> ListPhotos();
        Task<PhotoDto> GetPhoto(int id);
        Task<ServiceResponse> CreatePhoto(PhotoDto photoDto, IFormFile photoFile); // Updated to include file upload
        Task<ServiceResponse> UpdatePhotoDetails(int id, PhotoDto photoDto);
        Task<ServiceResponse> DeletePhoto(int id);
        Task<ServiceResponse> CreatePhoto(PhotoDto photoDto);
    }
}
