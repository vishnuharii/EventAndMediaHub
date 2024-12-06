using EventAndMediaHub.Interface;
using EventAndMediaHub.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventAndMediaHub.Controllers
{
    public class PhotoPageController : Controller
    {
        private readonly IPhotoService _photoService;

        public PhotoPageController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            return View(await _photoService.ListPhotos());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            return View(await _photoService.GetPhoto(id));
        }

        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PhotoDto photoDto)
        {
            var response = await _photoService.CreatePhoto(photoDto);

            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("Details", "PhotoPage", new { id = response.CreatedId });
            }

            return View("Error", new ErrorViewModel { Errors = response.Messages });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var photoDto = await _photoService.GetPhoto(id);

            if (photoDto == null)
            {
                return View("Error");
            }

            return View(photoDto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, PhotoDto photoDto)
        {
            var response = await _photoService.UpdatePhotoDetails(id, photoDto);

            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                return RedirectToAction("Details", "PhotoPage", new { id = id });
            }

            return View("Error", new ErrorViewModel { Errors = response.Messages });
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var photoDto = await _photoService.GetPhoto(id);

            if (photoDto == null)
            {
                return View("Error");
            }

            return View(photoDto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _photoService.DeletePhoto(id);

            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("List", "PhotoPage");
            }

            return View("Error");
        }
    }
}
