using EventAndMediaHub.Interface;
using EventAndMediaHub.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventAndMediaHub.Controllers
{
    public class CategoryPageController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryPageController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            return View(await _categoryService.ListCategories());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            return View(await _categoryService.GetCategory(id));
        }

        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDto categoryDto)
        {
            var response = await _categoryService.CreateCategory(categoryDto);

            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("Details", "CategoryPage", new { id = response.CreatedId });
            }

            return View("Error", new ErrorViewModel { Errors = response.Messages });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var categoryDto = await _categoryService.GetCategory(id);

            if (categoryDto == null)
            {
                return View("Error");
            }

            return View(categoryDto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, CategoryDto categoryDto)
        {
            var response = await _categoryService.UpdateCategory(id, categoryDto);

            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                return RedirectToAction("Details", "CategoryPage", new { id = id });
            }

            return View("Error", new ErrorViewModel { Errors = response.Messages });
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var categoryDto = await _categoryService.GetCategory(id);

            if (categoryDto == null)
            {
                return View("Error");
            }

            return View(categoryDto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _categoryService.DeleteCategory(id);

            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("List", "CategoryPage");
            }

            return View("Error");
        }
    }
}
