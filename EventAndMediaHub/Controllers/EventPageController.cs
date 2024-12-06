using EventAndMediaHub.Interface;
using EventAndMediaHub.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventAndMediaHub.Controllers
{
    public class EventPageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly IEventService _eventService;

        public EventPageController(IEventService eventService)
        {
            _eventService = eventService;
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            return View(await _eventService.ListEvents());
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            return View(await _eventService.GetEvent(id));
        }
        public ActionResult New()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(EventDto eventDto)
        {
            ServiceResponse response = await _eventService.CreateEvent(eventDto);

            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("Details", "EventPage", new { id = response.CreatedId });
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            EventDto? EventDto = await _eventService.GetEvent(id);

            if (EventDto == null)
            {
                return View("Error");
            }
            else
            {
                return View(EventDto);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, EventDto eventDto)
        {
            ServiceResponse response = await _eventService.UpdateEventDetails(id, eventDto);

            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                return RedirectToAction("Details", "EventPage", new { id = id });
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            EventDto? eventDto = await _eventService.GetEvent(id);
            if (eventDto == null)
            {
                return View("Error");
            }
            else
            {
                return View(eventDto);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResponse response = await _eventService.Deleteevent(id);

            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("List", "EventPage");
            }
            else
            {
                return View("Error");
            }
        }
    }
}
