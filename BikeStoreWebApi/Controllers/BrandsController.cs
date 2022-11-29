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

            return Ok(brands);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BrandDto>> GetBrandById(int id)
        {
            if (id == 0)
                return BadRequest();

            var brand = await _brandService.GetBrandById(id);

            return Ok(brand);
        }

        [Authorize(Roles = Roles.Admin, AuthenticationSchemes = AuthSchemes.JwtBearer)]
        [HttpPost("")]
        public async Task<ActionResult<BrandDto>> CreateBrand([FromBody] SaveBrandDto saveBrandDto)
        {
            var newBrand = await _brandService.CreateBrand(saveBrandDto);

            var brand = await _brandService.GetBrandById(newBrand.BrandId);

            return CreatedAtAction(nameof(CreateBrand), brand);
        }

        [Authorize(Roles = Roles.Admin, AuthenticationSchemes = AuthSchemes.JwtBearer)]
        [HttpPut("{id}")]
        public async Task<ActionResult<BrandDto>> UpdateBrand(int id, [FromBody] SaveBrandDto saveBrandDto)
        {
            if (id == 0)
                return BadRequest();

            if (!(await _brandService.UpdateBrand(id, saveBrandDto)))
            {
                return NotFound();
            }

            var updatedBrand = await _brandService.GetBrandById(id);

            return Ok(updatedBrand);
        }

        [Authorize(Roles = Roles.Admin, AuthenticationSchemes = AuthSchemes.JwtBearer)]
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
