using EventAndMediaHub.Interface;
using EventAndMediaHub.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventAndMediaHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationAPIController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationAPIController(ILocationService locationService)
        {
            _locationService = locationService;
        }
        [HttpGet("List")]
        public async Task<IEnumerable<LocationDto>> ListLocations()
        {
            return await _locationService.ListLocations();
        }
        [HttpGet("{id}")]
        public async Task<LocationDto> GetLocation(int id)
        {
            return await _locationService.GetLocation(id);
        }
        [HttpPost("Add")]
        public async Task<ServiceResponse> CreateLocation(LocationDto locationDto)
        {
            return await _locationService.CreateLocation(locationDto);
        }
        [HttpPut("Update/{id}")]
        public async Task<ServiceResponse> UpdateLocationDetails(int id, LocationDto locationDto)
        {
            return await _locationService.UpdateLocationDetails(id, locationDto);
        }
        [HttpDelete("Delete/{id}")]
        public async Task<ServiceResponse> DeleteLocation(int id)
        {
            return await _locationService.DeleteLocation(id);
        }

    }
}
