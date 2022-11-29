using BikeStore.Core.Models;
using BikeStoreWebApi.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BikeStore.Core.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAll();
        Task<OrderDto> GetById(int id);
        Task<OrderDto> CreateOrder(SaveOrderDto orderToCreate);
        Task<bool> CompleteOrder(int id);
    }
}
