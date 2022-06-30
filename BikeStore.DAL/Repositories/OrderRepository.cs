using BikeStore.Core.Models;
using BikeStore.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
                    .ThenInclude(b => b.Category)
                .Include(o => o.Bike)
                    .ThenInclude(b => b.Brand)
                .Include(o => o.User)
                .Select(o => new Order
                {
                    Bike = new Bike
                    {
                        BikeId = o.Bike.BikeId,
                        Name = o.Bike.Name,
                        ModelYear = o.Bike.ModelYear,
                        Description = o.Bike.Description,
                        Brand = o.Bike.Brand,
                        BrandId = o.Bike.BrandId,
                        Category = o.Bike.Category,
                        CategoryId = o.Bike.CategoryId,
                        Price = o.Bike.Price
                    },
                    User = o.User,
                    CreatedAt = o.CreatedAt,
                    IsCompleted = o.IsCompleted,
                    Id = o.Id
                })
                .ToListAsync();
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
