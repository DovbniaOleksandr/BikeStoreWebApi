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
    public class BikesController: ControllerBase
    {
        private readonly IBikeService _bikeService;
        private readonly IMapper _mapper;

        public BikesController(IBikeService bikeService, IMapper mapper)
        {
            _bikeService = bikeService;
            _mapper = mapper;
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<BikeDto>>> GetAllBikes()
        {
            var bikes = await _bikeService.GetAllBikesWithCategoryAndBrand();

            return Ok(bikes);
        }

        [HttpPost("filter")]
        public async Task<ActionResult<IEnumerable<BikeDto>>> FilterBikes([FromBody] BikeFilters filters)
        {
            var bikes = await _bikeService.FilterBikes(filters);

            return Ok(bikes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BikeDto>> GetBikeById(int id)
        {
            if (id == 0)
                return BadRequest();

            var bike = await _bikeService.GetBikeWithCategoryAndBrand(id);

            if (bike == null)
                return NotFound();

            return Ok(bike);
        }

        [Authorize(Roles = Roles.Admin, AuthenticationSchemes = AuthSchemes.JwtBearer)]
        [HttpPost]
        public async Task<ActionResult<BikeDto>> CreateBike([FromBody] SaveBikeDto saveBikeDto)
        {
            var newBike = await _bikeService.CreateBike(saveBikeDto);

            return CreatedAtAction(nameof(CreateBike), newBike);
        }

        [Authorize(Roles = Roles.Admin, AuthenticationSchemes = AuthSchemes.JwtBearer)]
        [HttpPut("{id}")]
        public async Task<ActionResult<BikeDto>> UpdateBike(int id, [FromBody] SaveBikeDto saveBikeDto)
        {
            if (id == 0)
                return BadRequest();

            if(!(await _bikeService.UpdateBike(id, saveBikeDto)))
            {
                return NotFound();
            }

            var updatedBike = await _bikeService.GetBikeWithCategoryAndBrand(id);

            return Ok(updatedBike);
        }

        [Authorize(Roles = Roles.Admin, AuthenticationSchemes = AuthSchemes.JwtBearer)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBike(int id)
        {
            if (id == 0)
                return BadRequest();

            var bike = await _bikeService.GetBikeById(id);

            if (bike == null)
                return NotFound();

            var bikeToDelete = _mapper.Map<BikeDto, Bike>(bike);

            await _bikeService.DeleteBike(bikeToDelete);

            return NoContent();
        }

    }
}
