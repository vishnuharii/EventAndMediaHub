using EventAndMediaHub.Models;

namespace EventAndMediaHub.Interface
{
    public interface IPhotoService
    {
        Task<IEnumerable<PhotoDto>> ListPhotos();
        Task<PhotoDto> GetPhoto(int id);
        Task<ServiceResponse> CreatePhoto(PhotoDto photoDto);
        Task<ServiceResponse> UpdatePhotoDetails(int id, PhotoDto photoDto);
        Task<ServiceResponse> DeletePhoto(int id);
    }
}
