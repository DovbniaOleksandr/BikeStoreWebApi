using AutoMapper;
using BikeStore.Core.Models;
using BikeStore.Core.Services;
using BikeStoreEF;
using BikeStoreWebApi.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BikeStore.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CategoryDto> CreateCategory(SaveCategoryDto newCategory)
        {
            var categoryToCreate = _mapper.Map<SaveCategoryDto, Category>(newCategory);

            await _unitOfWork.Categories.AddAsync(categoryToCreate);
            await _unitOfWork.SaveAsync();

            var categoryDto = _mapper.Map<Category, CategoryDto>(categoryToCreate);

            return categoryDto;
        }

        public async Task DeleteCategory(CategoryDto category)
        {
            var categoryToDelete = _mapper.Map<CategoryDto, Category>(category);

            _unitOfWork.Categories.Remove(categoryToDelete);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategories()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();

            return _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto> GetCategoryById(Guid id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            return _mapper.Map<Category, CategoryDto>(category);
        }

        public async Task<bool> UpdateCategory(Guid id, SaveCategoryDto category)
        {
            if (category == null)
            {
                throw new ArgumentNullException();
            }

            var categoryToBeUpdated = await _unitOfWork.Categories.GetWithBikesByIdAsync(id);

            if (categoryToBeUpdated == null)
            {
                return false;
            }

            categoryToBeUpdated.Name = category.Name;

            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
