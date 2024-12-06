using EventAndMediaHub.Interface;
using EventAndMediaHub.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventAndMediaHub.Controllers
{
    public class UserPageController : Controller
    {
        private readonly IUserService _userService;

        public UserPageController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            return View(await _userService.ListUsers());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            return View(await _userService.GetUser(id));
        }

        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserDto userDto)
        {
            var response = await _userService.CreateUser(userDto);

            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("Details", "UserPage", new { id = response.CreatedId });
            }
            else
            {
                return View("Error", new ErrorViewModel { Errors = response.Messages });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userDto = await _userService.GetUser(id);

            if (userDto == null)
            {
                return View("Error");
            }

            return View(userDto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, UserDto userDto)
        {
            var response = await _userService.UpdateUserDetails(id, userDto);

            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                return RedirectToAction("Details", "UserPage", new { id = id });
            }

            return View("Error", new ErrorViewModel { Errors = response.Messages });
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var userDto = await _userService.GetUser(id);

            if (userDto == null)
            {
                return View("Error");
            }

            return View(userDto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _userService.DeleteUser(id);

            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("List", "UserPage");
            }

            return View("Error");
        }
    }
}
