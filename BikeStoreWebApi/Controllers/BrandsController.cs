using AutoMapper;
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
    public class BrandsController: ControllerBase
    {
        private IBrandService _brandService;
        private IMapper _mapper;

        public BrandsController(IBrandService brandService, IMapper mapper)
        {
            _brandService = brandService;
            _mapper = mapper;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<BrandDto>>> GetAllBrands()
        {
            var brands = await _brandService.GetAllBrands();
            var brandDtos = _mapper.Map<IEnumerable<Brand>, IEnumerable<BrandDto>>(brands);

            return Ok(brandDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BrandDto>> GetBrandById(int id)
        {
            var brand = await _brandService.GetBrandById(id);
            var brandDto = _mapper.Map<Brand, BrandDto>(brand);

            return Ok(brandDto);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost("")]
        public async Task<ActionResult<BrandDto>> CreateBrand([FromBody] SaveBrandDto saveBrandDto)
        {
            var validator = new BrandValidator();
            var validationResult = await validator.ValidateAsync(saveBrandDto);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var brandToCreate = _mapper.Map<SaveBrandDto, Brand>(saveBrandDto);

            var newBrand = await _brandService.CreateBrand(brandToCreate);

            var brand = await _brandService.GetBrandById(newBrand.BrandId);

            var brandDto = _mapper.Map<Brand, BrandDto>(brand);

            return Ok(brandDto);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("{id}")]
        public async Task<ActionResult<BrandDto>> UpdateBrand(int id, [FromBody] SaveBrandDto saveBrandDto)
        {
            var validator = new BrandValidator();
            var validationResult = await validator.ValidateAsync(saveBrandDto);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var brandToBeUpdated = await _brandService.GetBrandById(id);

            if (brandToBeUpdated == null)
                return NotFound();

            var brand = _mapper.Map<SaveBrandDto, Brand>(saveBrandDto);

            await _brandService.UpdateBrand(brandToBeUpdated, brand);

            var updatedBrand = await _brandService.GetBrandById(id);
            var updatedBrandDto = _mapper.Map<Brand, BrandDto>(updatedBrand);

            return Ok(updatedBrandDto);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            if (id == 0)
                return BadRequest();

            var brand = await _brandService.GetBrandById(id);

            if (brand == null)
                return NotFound();

            await _brandService.DeleteBrand(brand);

            return NoContent();
        }
    }
}
