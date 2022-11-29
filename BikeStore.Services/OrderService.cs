using AutoMapper;
using BikeStore.Core.Models;
using BikeStore.Core.Services;
using BikeStoreEF;
using BikeStoreWebApi.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BikeStore.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> CompleteOrder(int id)
        {
            var order = await _unitOfWork.Orders.GetById(id);

            if (order == null)
                return false;

            order.IsCompleted = true;
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task<OrderDto> CreateOrder(SaveOrderDto orderToCreate)
        {
            var order = _mapper.Map<SaveOrderDto, Order>(orderToCreate);

            order.CreatedAt = DateTime.Now;

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<Order, OrderDto>(order);
        }

        public async Task<IEnumerable<OrderDto>> GetAll()
        {
            var orders = await _unitOfWork.Orders.GetAll();

            return _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto> GetById(int id)
        {
            var order = await _unitOfWork.Orders.GetById(id);

            return _mapper.Map<Order, OrderDto>(order);
        }
    }
}
