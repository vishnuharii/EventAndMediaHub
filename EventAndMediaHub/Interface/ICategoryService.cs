using EventAndMediaHub.Models;

namespace EventAndMediaHub.Interface
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> ListCategories();
        Task<CategoryDto> GetCategory(int id);
        Task<ServiceResponse> CreateCategory(CategoryDto categoryDto);
        Task<ServiceResponse> UpdateCategory(int id, CategoryDto categoryDto);
        Task<ServiceResponse> DeleteCategory(int id);
    }
}
