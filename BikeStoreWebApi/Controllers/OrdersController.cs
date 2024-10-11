using AutoMapper;
using BikeStore.Core.Enums;
using BikeStore.Core.Models;
using BikeStore.Core.Services;
using BikeStoreWebApi.DTOs.Order;
using BikeStoreWebApi.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BikeStoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IMapper mapper, IOrderService orderService)
        {
            _mapper = mapper;
            _orderService = orderService;
        }

        [Authorize(Roles = Roles.Admin, AuthenticationSchemes = AuthSchemes.JwtBearer)]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest();

            var order = await _orderService.GetById(id);

            if(order == null)
            {
                return NotFound(id);
            }

            return Ok(order);
        }

        [Authorize(Roles = Roles.Admin, AuthenticationSchemes = AuthSchemes.JwtBearer)]
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrders()
        {
            var orders = await _orderService.GetAll();

            return Ok(orders);
        }

        [HttpPost("create"), Authorize(AuthenticationSchemes = AuthSchemes.JwtBearer)]
        public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] SaveOrderDto saveOrderDto)
        {
            var loggedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!User.IsInRole(Roles.Admin) && loggedUserId != saveOrderDto.UserId.ToString())
            {
                return StatusCode(403, "You are not allowed to edit this user's information.");
            }

            var newOrder = await _orderService.CreateOrder(saveOrderDto);

            var order = await _orderService.GetById(newOrder.Id);

            return CreatedAtAction(nameof(CreateOrder), order);
        }

        [Authorize(Roles = Roles.Admin, AuthenticationSchemes = AuthSchemes.JwtBearer)]
        [HttpPost("complete/{id}")]
        public async Task<ActionResult<OrderDto>> CompleteOrder(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest();

            if (!(await _orderService.CompleteOrder(id)))
                return NotFound();

            return NoContent();
        }
    }
}
