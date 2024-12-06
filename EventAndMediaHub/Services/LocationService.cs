using EventAndMediaHub.Data;
using EventAndMediaHub.Interface;
using EventAndMediaHub.Models;
using Microsoft.EntityFrameworkCore;

namespace EventAndMediaHub.Services
{
    public class LocationService : ILocationService
    {
        private readonly ApplicationDbContext _context;

        public LocationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LocationDto>> ListLocations()
        {
            var locations = await _context.Locations.ToListAsync();
            var locationDtos = locations.Select(location => new LocationDto
            {
                LocationId = location.LocationId,
                LocationName = location.LocationName,
                Address = location.Address,
                Capacity = location.Capacity
            }).ToList();

            return locationDtos;
        }

        public async Task<LocationDto> GetLocation(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            return location == null ? null : new LocationDto
            {
                LocationId = location.LocationId,
                LocationName = location.LocationName,
                Address = location.Address,
                Capacity = location.Capacity
            };
        }

        public async Task<ServiceResponse> CreateLocation(LocationDto locationDto)
        {
            var serviceResponse = new ServiceResponse();

            var location = new Location
            {
                LocationName = locationDto.LocationName,
                Address = locationDto.Address,
                Capacity = locationDto.Capacity
            };

            _context.Locations.Add(location);
            await _context.SaveChangesAsync();

            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            serviceResponse.CreatedId = location.LocationId;
            serviceResponse.Messages.Add($"Location created successfully: {location.LocationName}");

            return serviceResponse;
        }

        public async Task<ServiceResponse> UpdateLocationDetails(int id, LocationDto locationDto)
        {
            var serviceResponse = new ServiceResponse();

            var existingLocation = await _context.Locations.FindAsync(id);
            if (existingLocation == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("Location not found.");
                return serviceResponse;
            }

            existingLocation.LocationName = locationDto.LocationName ?? existingLocation.LocationName;
            existingLocation.Address = locationDto.Address ?? existingLocation.Address;
            existingLocation.Capacity = locationDto.Capacity != 0 ? locationDto.Capacity : existingLocation.Capacity;

            await _context.SaveChangesAsync();

            serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
            serviceResponse.Messages.Add($"Location updated successfully: {existingLocation.LocationName}");

            return serviceResponse;
        }

        public async Task<ServiceResponse> DeleteLocation(int id)
        {
            var serviceResponse = new ServiceResponse();

            var location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("Location not found.");
                return serviceResponse;
            }

            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();

            serviceResponse.Status = ServiceResponse.ServiceStatus.Deleted;
            serviceResponse.Messages.Add($"Location deleted successfully: {location.LocationName}");

            return serviceResponse;
        }
    }
}
