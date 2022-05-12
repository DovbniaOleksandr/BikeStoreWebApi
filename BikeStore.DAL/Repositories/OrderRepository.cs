using BikeStore.Core.Models;
using BikeStore.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BikeStore.DAL.Repositories
{
    public class OrderRepository: Repository<Order>, IOrderRepository
    {
        private BikeStoreDBContext BikeStoreDBContext
        {
            get { return Context as BikeStoreDBContext; }
        }

        public OrderRepository(BikeStoreDBContext context)
            : base(context)
        { }

        public async Task<IEnumerable<Order>> GetAll()
        {
            return await BikeStoreDBContext.Orders
                .Include(o => o.Bike)
                .Include(o => o.User)
                .ToListAsync<Order>();
        }

        public async Task<Order> GetById(int id)
        {
            return await BikeStoreDBContext.Orders
                .Include(o => o.Bike)
                .Include(o => o.User)
                .SingleOrDefaultAsync(o => o.Id == id);
        }
    }
}
