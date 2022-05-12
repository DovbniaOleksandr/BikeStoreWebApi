using AutoMapper;
using BikeStore.Core.Enums;
using BikeStore.Core.Models;
using BikeStore.Core.Services;
using BikeStoreWebApi.DTOs;
using BikeStoreWebApi.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeStoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController: ControllerBase
    {
        private ICategoryService _categoryService;
        private IMapper _mapper;

        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategories();
            var categoryDtos = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDto>>(categories);

            return Ok(categoryDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryById(id);
            var categoryDto = _mapper.Map<Category, CategoryDto>(category);

            return Ok(categoryDto);
        }

        [Authorize(Roles = Roles.Admin, AuthenticationSchemes = AuthSchemes.JwtBearer)]
        [HttpPost("")]
        public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] SaveCategoryDto saveCategoryDto)
        {
            var validator = new CategoryValidator();
            var validationResult = await validator.ValidateAsync(saveCategoryDto);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var categoryToCreate = _mapper.Map<SaveCategoryDto, Category>(saveCategoryDto);

            var newCategory = await _categoryService.CreateCategory(categoryToCreate);

            var category = await _categoryService.GetCategoryById(newCategory.CategoryId);

            var categoryDto = _mapper.Map<Category, CategoryDto>(category);

            return Ok(categoryDto);
        }

        [Authorize(Roles = Roles.Admin, AuthenticationSchemes = AuthSchemes.JwtBearer)]
        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDto>> UpdateCategory(int id, [FromBody] SaveCategoryDto saveCategoryDto)
        {
            var validator = new CategoryValidator();
            var validationResult = await validator.ValidateAsync(saveCategoryDto);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var category = _mapper.Map<SaveCategoryDto, Category>(saveCategoryDto);

            if (!(await _categoryService.UpdateCategory(id, category)))
            {
                return NotFound();
            }

            var updatedCategory = await _categoryService.GetCategoryById(id);
            var updatedCategoryDto = _mapper.Map<Category, CategoryDto>(updatedCategory);

            return Ok(updatedCategoryDto);
        }

        [Authorize(Roles = Roles.Admin, AuthenticationSchemes = AuthSchemes.JwtBearer)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (id == 0)
                return BadRequest();

            var category = await _categoryService.GetCategoryById(id);

            if (category == null)
                return NotFound();

            await _categoryService.DeleteCategory(category);

            return NoContent();
        }
    }
}
