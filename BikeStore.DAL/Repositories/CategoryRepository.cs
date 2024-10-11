using BikeStore.Core.Models;
using BikeStore.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BikeStore.DAL.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private BikeStoreDBContext BikeStoreDBContext
        {
            get { return Context as BikeStoreDBContext; }
        }

        public CategoryRepository(BikeStoreDBContext context)
            : base(context)
        { }

        public async Task<IEnumerable<Category>> GetAllWithBikesAsync()
        {
            return await BikeStoreDBContext.Categories
               .Include(c => c.Bikes)
               .ToListAsync();
        }

        public async Task<Category> GetWithBikesByIdAsync(Guid id)
        {
            return await BikeStoreDBContext.Categories
                .Include(c => c.Bikes)
                .SingleOrDefaultAsync(b => b.Id == id);
        }
    }
}
