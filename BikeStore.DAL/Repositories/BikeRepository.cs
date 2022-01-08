using BikeStore.Core.Models;
using BikeStore.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BikeStore.DAL.Repositories
{
    public class BikeRepository: Repository<Bike>, IBikeRepository
    {
        private BikeStoreDBContext BikeStoreDBContext
        {
            get { return Context as BikeStoreDBContext; }
        }

        public BikeRepository(BikeStoreDBContext context)
            :base(context)
        { }

        public async Task<IEnumerable<Bike>> FindAllWithBrandAndCategoryAsync(Expression<Func<Bike, bool>> predicate)
        {
            return await BikeStoreDBContext.Bikes
                .Include(b => b.Category)
                .Include(b => b.Brand)
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Bike>> GetAllWithBrandAndCategoryAsync()
        {
            return await BikeStoreDBContext.Bikes
                .Include(b => b.Category)
                .Include(b => b.Brand)
                .ToListAsync();
        }

        public async Task<Bike> GetWithBrandAndCategoryByIdAsync(int id)
        {
            return await BikeStoreDBContext.Bikes
                .Include(b => b.Category)
                .Include(b => b.Brand)
                .SingleOrDefaultAsync(b => b.BikeId == id);
        }
    }
}
