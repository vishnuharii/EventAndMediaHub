using EventAndMediaHub.Data;
using EventAndMediaHub.Interface;
using EventAndMediaHub.Models;
using Microsoft.EntityFrameworkCore;

namespace EventAndMediaHub.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryDto>> ListCategories()
        {
            var categories = await _context.Categories.ToListAsync();

            var categoryDtos = categories.Select(category => new CategoryDto()
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName
            }).ToList();

            return categoryDtos;
        }

        public async Task<CategoryDto> GetCategory(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null)
            {
                return null;
            }

            return new CategoryDto()
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName
            };
        }

        public async Task<ServiceResponse> CreateCategory(CategoryDto categoryDto)
        {
            var serviceResponse = new ServiceResponse();

            var category = new Category()
            {
                CategoryName = categoryDto.CategoryName
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            serviceResponse.CreatedId = category.CategoryId;
            serviceResponse.Messages.Add($"Category created successfully: {category.CategoryName}");

            return serviceResponse;
        }

        public async Task<ServiceResponse> UpdateCategory(int id, CategoryDto categoryDto)
        {
            var serviceResponse = new ServiceResponse();

            var existingCategory = await _context.Categories.FindAsync(id);
            if (existingCategory == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("Category not found.");
                return serviceResponse;
            }

            existingCategory.CategoryName = categoryDto.CategoryName ?? existingCategory.CategoryName;

            await _context.SaveChangesAsync();

            serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
            serviceResponse.Messages.Add($"Category updated successfully: {existingCategory.CategoryName}");

            return serviceResponse;
        }

        public async Task<ServiceResponse> DeleteCategory(int id)
        {
            var serviceResponse = new ServiceResponse();

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("Category not found.");
                return serviceResponse;
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            serviceResponse.Status = ServiceResponse.ServiceStatus.Deleted;
            serviceResponse.Messages.Add("Category deleted successfully.");

            return serviceResponse;
        }
    }
}
