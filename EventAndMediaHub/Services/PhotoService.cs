using EventAndMediaHub.Data;
using EventAndMediaHub.Interface;
using EventAndMediaHub.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace EventAndMediaHub.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PhotoService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IEnumerable<PhotoDto>> ListPhotos()
        {
            var photos = await _context.Photos.ToListAsync();

            var photoDtos = photos.Select(photo => new PhotoDto()
            {
                PhotoId = photo.PhotoId,
                Title = photo.Title,
                Description = photo.Description,
                Price = photo.Price,
                UploadDate = photo.UploadDate,
                UserId = photo.UserId,
                UserName = photo.UserName,
                PhotoPath = photo.PhotoPath, // Include file path
            }).ToList();

            return photoDtos;
        }

        public async Task<PhotoDto?> GetPhoto(int id)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.PhotoId == id);

            if (photo == null)
            {
                return null;
            }

            return new PhotoDto()
            {
                PhotoId = photo.PhotoId,
                Title = photo.Title,
                Description = photo.Description,
                Price = photo.Price,
                UploadDate = photo.UploadDate,
                UserId = photo.UserId,
                UserName = photo.UserName,
                PhotoPath = photo.PhotoPath, // Include file path
            };
        }

        public async Task<ServiceResponse> CreatePhoto(PhotoDto photoDto, IFormFile photoFile)
        {
            var serviceResponse = new ServiceResponse();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == photoDto.UserId);
            if (user == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("User not found.");
                return serviceResponse;
            }

            string filePath = null;
            if (photoFile != null && photoFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + photoFile.FileName;
                filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photoFile.CopyToAsync(stream);
                }
            }

            var photo = new Photo()
            {
                Title = photoDto.Title,
                Description = photoDto.Description,
                Price = photoDto.Price,
                UploadDate = photoDto.UploadDate,
                UserId = photoDto.UserId,
                UserName = user.UserName,
                PhotoPath = filePath != null ? $"/uploads/{Path.GetFileName(filePath)}" : null,
            };

            _context.Photos.Add(photo);
            await _context.SaveChangesAsync();

            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            serviceResponse.CreatedId = photo.PhotoId;
            serviceResponse.Messages.Add($"Photo created successfully: {photo.Title}");

            return serviceResponse;
        }

        public async Task<ServiceResponse> UpdatePhotoDetails(int id, PhotoDto photoDto)
        {
            var serviceResponse = new ServiceResponse();

            var existingPhoto = await _context.Photos.FindAsync(id);
            if (existingPhoto == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("Photo not found.");
                return serviceResponse;
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == photoDto.UserId);
            if (user == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("User not found.");
                return serviceResponse;
            }

            existingPhoto.Title = photoDto.Title ?? existingPhoto.Title;
            existingPhoto.Description = photoDto.Description ?? existingPhoto.Description;
            existingPhoto.Price = photoDto.Price != 0 ? photoDto.Price : existingPhoto.Price;
            existingPhoto.UserId = photoDto.UserId;
            existingPhoto.UserName = user.UserName;

            await _context.SaveChangesAsync();

            serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
            serviceResponse.Messages.Add($"Photo updated successfully: {existingPhoto.Title}");

            return serviceResponse;
        }

        public async Task<ServiceResponse> DeletePhoto(int id)
        {
            var serviceResponse = new ServiceResponse();

            var photo = await _context.Photos.FindAsync(id);
            if (photo == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("Photo not found.");
                return serviceResponse;
            }

            if (!string.IsNullOrEmpty(photo.PhotoPath))
            {
                var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, photo.PhotoPath.TrimStart('/'));
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }

            _context.Photos.Remove(photo);
            await _context.SaveChangesAsync();

            serviceResponse.Status = ServiceResponse.ServiceStatus.Deleted;
            serviceResponse.Messages.Add("Photo deleted successfully.");

            return serviceResponse;
        }

        public Task<ServiceResponse> CreatePhoto(PhotoDto photoDto)
        {
            throw new NotImplementedException();
        }
    }
}
