using BikeStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BikeStore.Core.Services
{
    public interface ICategoryService
    {
        Task<Category> GetCategoryById(int id);
        Task<IEnumerable<Category>> GetAllCategories();
        Task<Category> CreateCategory(Category newCategory);
        Task<bool> UpdateCategory(int id, Category category);
        Task DeleteCategory(Category category);
    }
}
