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

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<BikeDto>>> GetAllBikes()
        {
            var bikes = await _bikeService.GetAllBikesWithCategoryAndBrand();
            var bikeDtos = _mapper.Map<IEnumerable<Bike>, IEnumerable<BikeDto>>(bikes);

            return Ok(bikeDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BikeDto>> GetBikeById(int id)
        {
            var bike = await _bikeService.GetBikeWithCategoryAndBrand(id);
            var bikeDto = _mapper.Map<Bike, BikeDto>(bike);

            return Ok(bikeDto);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost("")]
        public async Task<ActionResult<BikeDto>> CreateBike([FromBody] SaveBikeDto saveBikeDto)
        {
            var validator = new BikeValidator();
            var validationResult = await validator.ValidateAsync(saveBikeDto);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var bikeToCreate = _mapper.Map<SaveBikeDto, Bike>(saveBikeDto);

            var newBike = await _bikeService.CreateBike(bikeToCreate);

            var bike = await _bikeService.GetBikeWithCategoryAndBrand(newBike.BikeId);

            var bikeDto = _mapper.Map<Bike, BikeDto>(bike);

            return Ok(bikeDto);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("{id}")]
        public async Task<ActionResult<BikeDto>> UpdateBike(int id, [FromBody] SaveBikeDto saveBikeDto)
        {
            var validator = new BikeValidator();
            var validationResult = await validator.ValidateAsync(saveBikeDto);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var bike = _mapper.Map<SaveBikeDto, Bike>(saveBikeDto);

            if(!(await _bikeService.UpdateBike(id, bike)))
            {
                return NotFound();
            }

            var updatedBike = await _bikeService.GetBikeWithCategoryAndBrand(id);
            var updatedBikeDto = _mapper.Map<Bike, BikeDto>(updatedBike);

            return Ok(updatedBikeDto);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBike(int id)
        {
            if (id == 0)
                return BadRequest();

            var bike = await _bikeService.GetBikeById(id);

            if (bike == null)
                return NotFound();

            await _bikeService.DeleteBike(bike);

            return NoContent();
        }

    }
}
