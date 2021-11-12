using BikeStore.Core.Models;
using BikeStore.Core.Services;
using BikeStoreEF;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BikeStore.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<Category> CreateCategory(Category newCategory)
        {
            await _unitOfWork.Categories.AddAsync(newCategory);
            await _unitOfWork.SaveAsync();
            return newCategory;
        }

        public async Task DeleteCategory(Category category)
        {
            _unitOfWork.Categories.Remove(category);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _unitOfWork.Categories.GetAllAsync();
        }

        public async Task<Category> GetCategoryById(int id)
        {
            return await _unitOfWork.Categories.GetByIdAsync(id);
        }

        public async Task UpdateCategory(Category categoryToBeUpdated, Category category)
        {
            categoryToBeUpdated.CategoryId = category.CategoryId;
            categoryToBeUpdated.Name = category.Name;

            await _unitOfWork.SaveAsync();
        }
    }
}
