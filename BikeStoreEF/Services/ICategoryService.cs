using BikeStore.Core.Models;
using BikeStoreWebApi.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BikeStore.Core.Services
{
    public interface ICategoryService
    {
        Task<CategoryDto> GetCategoryById(Guid id);
        Task<IEnumerable<CategoryDto>> GetAllCategories();
        Task<CategoryDto> CreateCategory(SaveCategoryDto newCategory);
        Task<bool> UpdateCategory(Guid id, SaveCategoryDto category);
        Task DeleteCategory(CategoryDto category);
    }
}
