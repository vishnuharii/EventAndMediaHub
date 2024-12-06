using EventAndMediaHub.Data;
using EventAndMediaHub.Interface;
using EventAndMediaHub.Models;
using Microsoft.EntityFrameworkCore;

namespace EventAndMediaHub.Services
{
    public class EventService : IEventService
    {
        private readonly ApplicationDbContext _context;

        public EventService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EventDto>> ListEvents()
        {
            List<Event> events = await _context.Events.ToListAsync();

            List<EventDto> eventDtos = new List<EventDto>();
            foreach (Event eventItem in events)
            {
                eventDtos.Add(new EventDto()
                {
                    EventId = eventItem.EventId,
                    EventName = eventItem.EventName,
                    Date = eventItem.Date,
                    Description = eventItem.Description,
                    LocationId = eventItem.LocationId,
                    LocationName = eventItem.LocationName,
                    UserId = eventItem.UserId,
                    UserName = eventItem.UserName,
                });
            }

            return eventDtos;
        }

        public async Task<EventDto> GetEvent(int id)
        {
            var eventItem = await _context.Events.FirstOrDefaultAsync(e => e.EventId == id);

            if (eventItem == null)
            {
                return null;
            }

            return new EventDto()
            {
                EventId = eventItem.EventId,
                EventName = eventItem.EventName,
                Date = eventItem.Date,
                Description = eventItem.Description,
                LocationId = eventItem.LocationId,
                LocationName = eventItem.LocationName,
                UserId = eventItem.UserId,
                UserName = eventItem.UserName,
            };
        }

        public async Task<ServiceResponse> CreateEvent(EventDto eventDto)
        {
            ServiceResponse serviceResponse = new();

            // Check if the provided LocationName exists in the Locations table
            var location = await _context.Locations
                                          .FirstOrDefaultAsync(l => l.LocationName == eventDto.LocationName);
            if (location == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("Location not found. Please provide a valid LocationName.");
                return serviceResponse;
            }

            // Check if the provided UserName exists in the Users table
            var user = await _context.Users
                                     .FirstOrDefaultAsync(u => u.UserName == eventDto.UserName);
            if (user == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("User not found. Please provide a valid UserName.");
                return serviceResponse;
            }

            Event eventItem = new Event()
            {
                EventName = eventDto.EventName,
                Date = eventDto.Date,
                Description = eventDto.Description,
                LocationId = location.LocationId, // Valid LocationId from the table
                LocationName = location.LocationName,
                UserId = user.UserId,              // Valid UserId from the table
                UserName = user.UserName,
            };

            _context.Events.Add(eventItem);
            await _context.SaveChangesAsync();

            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            serviceResponse.CreatedId = eventItem.EventId;
            serviceResponse.Messages.Add($"Event created successfully: {eventItem.EventName}");

            return serviceResponse;
        }

        public async Task<ServiceResponse> UpdateEventDetails(int id, EventDto eventDto)
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            var existingEvent = await _context.Events.FindAsync(id);
            if (existingEvent == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("Event not found.");
                return serviceResponse;
            }

            // Check if LocationName is being updated and if it exists in Locations table
            if (!string.IsNullOrEmpty(eventDto.LocationName) && eventDto.LocationName != existingEvent.LocationName)
            {
                var location = await _context.Locations
                                              .FirstOrDefaultAsync(l => l.LocationName == eventDto.LocationName);
                if (location == null)
                {
                    serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                    serviceResponse.Messages.Add("Location not found.");
                    return serviceResponse;
                }
                existingEvent.LocationId = location.LocationId;
                existingEvent.LocationName = location.LocationName;
            }

            // Check if UserName is being updated and if it exists in Users table
            if (!string.IsNullOrEmpty(eventDto.UserName) && eventDto.UserName != existingEvent.UserName)
            {
                var user = await _context.Users
                                         .FirstOrDefaultAsync(u => u.UserName == eventDto.UserName);
                if (user == null)
                {
                    serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                    serviceResponse.Messages.Add("User not found.");
                    return serviceResponse;
                }
                existingEvent.UserId = user.UserId;
                existingEvent.UserName = user.UserName;
            }

            // Update other properties
            existingEvent.EventName = eventDto.EventName ?? existingEvent.EventName;
            existingEvent.Date = eventDto.Date != default ? eventDto.Date : existingEvent.Date;
            existingEvent.Description = eventDto.Description ?? existingEvent.Description;

            // Save changes to the database
            await _context.SaveChangesAsync();

            serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
            serviceResponse.Messages.Add($"Event updated successfully: {existingEvent.EventName}");

            return serviceResponse;
        }

        public async Task<ServiceResponse> Deleteevent(int id)
        {
            ServiceResponse response = new();
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Event cannot be deleted because it does not exist.");
                return response;
            }

            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();
            response.Status = ServiceResponse.ServiceStatus.Deleted;
            response.Messages.Add("Event deleted successfully.");

            return response;
        }
    }
}
