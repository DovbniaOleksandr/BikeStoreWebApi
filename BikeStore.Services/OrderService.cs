using BikeStore.Core.Models;
using BikeStore.Core.Services;
using BikeStoreEF;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BikeStore.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<Order> CreateOrder(Order orderToCreate)
        {
            await _unitOfWork.Orders.AddAsync(orderToCreate);
            await _unitOfWork.SaveAsync();

            return orderToCreate;
        }

        public async Task<IEnumerable<Order>> GetAll()
        {
            return await _unitOfWork.Orders.GetAll();
        }

        public async Task<Order> GetById(int id)
        {
            return await _unitOfWork.Orders.GetById(id);
        }
    }
}
