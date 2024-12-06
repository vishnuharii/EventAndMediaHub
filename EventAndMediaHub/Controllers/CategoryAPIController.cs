using EventAndMediaHub.Interface;
using EventAndMediaHub.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventAndMediaHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryAPIController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryAPIController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("List")]
        public async Task<IEnumerable<CategoryDto>> ListCategories()
        {
            return await _categoryService.ListCategories();
        }

        [HttpGet("{id}")]
        public async Task<CategoryDto> GetCategory(int id)
        {
            return await _categoryService.GetCategory(id);
        }

        [HttpPost("Add")]
        public async Task<ServiceResponse> CreateCategory(CategoryDto categoryDto)
        {
            return await _categoryService.CreateCategory(categoryDto);
        }

        [HttpPut("Update/{id}")]
        public async Task<ServiceResponse> UpdateCategory(int id, CategoryDto categoryDto)
        {
            return await _categoryService.UpdateCategory(id, categoryDto);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ServiceResponse> DeleteCategory(int id)
        {
            return await _categoryService.DeleteCategory(id);
        }
    }
}
