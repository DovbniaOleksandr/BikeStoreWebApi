﻿using AutoMapper;
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
        public async Task<ActionResult<OrderDto>> GetOrderById(int id)
        {
            if (id == 0)
                return BadRequest();

            var order = await _orderService.GetById(id);

            if(order == null)
            {
                return NotFound(id);
            }

            var orderDto = _mapper.Map<Order, OrderDto>(order);

            return Ok(orderDto);
        }

        [Authorize(Roles = Roles.Admin, AuthenticationSchemes = AuthSchemes.JwtBearer)]
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrders()
        {
            var orders = await _orderService.GetAll();
            var orderDtos = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDto>>(orders);

            return Ok(orderDtos);
        }

        [HttpPost("create")]
        public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] SaveOrderDto saveOrderDto)
        {
            var orderToCreate = _mapper.Map<SaveOrderDto, Order>(saveOrderDto);

            var newOrder = await _orderService.CreateOrder(orderToCreate);

            var order = await _orderService.GetById(newOrder.BikeId);

            var orderDto = _mapper.Map<Order, OrderDto>(order);

            return CreatedAtAction(nameof(CreateOrder), orderDto);
        }

        [Authorize(Roles = Roles.Admin, AuthenticationSchemes = AuthSchemes.JwtBearer)]
        [HttpPost("complete/{id}")]
        public async Task<ActionResult<OrderDto>> CompleteOrder(int id)
        {
            if (id == 0)
                return BadRequest();

            var order = await _orderService.GetById(id);

            if (order == null)
                return NotFound();

            await _orderService.CompleteOrder(order);

            return NoContent();
        }
    }
}
