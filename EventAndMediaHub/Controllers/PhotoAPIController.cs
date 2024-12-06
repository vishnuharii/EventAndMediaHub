using EventAndMediaHub.Interface;
using EventAndMediaHub.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventAndMediaHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoAPIController : ControllerBase
    {
        private readonly IPhotoService _photoService;

        public PhotoAPIController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [HttpGet("List")]
        public async Task<IEnumerable<PhotoDto>> ListPhotos()
        {
            return await _photoService.ListPhotos();
        }

        [HttpGet("{id}")]
        public async Task<PhotoDto> GetPhoto(int id)
        {
            return await _photoService.GetPhoto(id);
        }

        [HttpPost("Add")]
        public async Task<ServiceResponse> CreatePhoto(PhotoDto photoDto)
        {
            return await _photoService.CreatePhoto(photoDto);
        }

        [HttpPut("Update/{id}")]
        public async Task<ServiceResponse> UpdatePhotoDetails(int id, PhotoDto photoDto)
        {
            return await _photoService.UpdatePhotoDetails(id, photoDto);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ServiceResponse> DeletePhoto(int id)
        {
            return await _photoService.DeletePhoto(id);
        }
    }
}
