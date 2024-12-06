using EventAndMediaHub.Data;
using EventAndMediaHub.Interface;
using EventAndMediaHub.Models;
using Microsoft.EntityFrameworkCore;

namespace EventAndMediaHub.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly ApplicationDbContext _context;

        public PhotoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PhotoDto>> ListPhotos()
        {
            var photos = await _context.Photos
                .ToListAsync();  // Fetch photos, without needing to include users explicitly

            var photoDtos = photos.Select(photo => new PhotoDto()
            {
                PhotoId = photo.PhotoId,
                Title = photo.Title,
                Description = photo.Description,
                Price = photo.Price,
                UploadDate = photo.UploadDate,
                UserId = photo.UserId,
                UserName = photo.UserName,  // Assuming UserName is stored correctly already
            }).ToList();

            return photoDtos;
        }

        public async Task<PhotoDto?> GetPhoto(int id)
        {
            var photo = await _context.Photos
                .FirstOrDefaultAsync(p => p.PhotoId == id);

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
                UserName = photo.UserName,  // Return the UserName (assuming it is populated correctly)
            };
        }

        public async Task<ServiceResponse> CreatePhoto(PhotoDto photoDto)
        {
            var serviceResponse = new ServiceResponse();

            // Ensure the user exists and retrieve the UserName
            var user = await _context.Users
                                     .FirstOrDefaultAsync(u => u.UserId == photoDto.UserId);  // Get user by UserId
            if (user == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("User not found.");
                return serviceResponse;
            }

            // Ensure the provided UserName matches the one from the UserId
            if (!string.IsNullOrEmpty(photoDto.UserName) && photoDto.UserName != user.UserName)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("The provided UserName does not match the UserId.");
                return serviceResponse;
            }

            var photo = new Photo()
            {
                Title = photoDto.Title,
                Description = photoDto.Description,
                Price = photoDto.Price,
                UploadDate = photoDto.UploadDate,
                UserId = photoDto.UserId,
                UserName = user.UserName,  // Automatically set UserName based on UserId
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

            // If UserId is being updated, fetch the new UserName
            if (photoDto.UserId != 0 && photoDto.UserId != existingPhoto.UserId)
            {
                var user = await _context.Users
                                         .FirstOrDefaultAsync(u => u.UserId == photoDto.UserId); // Fetch the user based on UserId
                if (user == null)
                {
                    serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                    serviceResponse.Messages.Add("User not found.");
                    return serviceResponse;
                }
                existingPhoto.UserId = photoDto.UserId;
                existingPhoto.UserName = user.UserName; // Automatically update the UserName based on the new UserId
            }

            // If UserName is being updated, verify it exists in the Users table
            if (!string.IsNullOrEmpty(photoDto.UserName) && photoDto.UserName != existingPhoto.UserName)
            {
                var user = await _context.Users
                                         .FirstOrDefaultAsync(u => u.UserName == photoDto.UserName); // Find user by UserName
                if (user == null)
                {
                    serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                    serviceResponse.Messages.Add("UserName does not exist.");
                    return serviceResponse;
                }
                existingPhoto.UserName = photoDto.UserName; // Update the UserName if it exists
            }

            // Update other properties
            existingPhoto.Title = photoDto.Title ?? existingPhoto.Title;
            existingPhoto.Description = photoDto.Description ?? existingPhoto.Description;
            existingPhoto.Price = photoDto.Price != 0 ? photoDto.Price : existingPhoto.Price;
            existingPhoto.UploadDate = photoDto.UploadDate != default ? photoDto.UploadDate : existingPhoto.UploadDate;

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

            _context.Photos.Remove(photo);
            await _context.SaveChangesAsync();

            serviceResponse.Status = ServiceResponse.ServiceStatus.Deleted;
            serviceResponse.Messages.Add("Photo deleted successfully.");

            return serviceResponse;
        }
    }
}
