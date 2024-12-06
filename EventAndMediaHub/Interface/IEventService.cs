using EventAndMediaHub.Models;

namespace EventAndMediaHub.Interface
{
    public interface IEventService
    {
        Task<IEnumerable<EventDto>> ListEvents();
        Task<EventDto> GetEvent(int id);
        Task<ServiceResponse> CreateEvent(EventDto eventDto);
        Task<ServiceResponse> UpdateEventDetails(int id, EventDto eventDto);
        Task<ServiceResponse> Deleteevent(int id);

    }
}
