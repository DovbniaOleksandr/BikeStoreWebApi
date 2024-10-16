﻿using BikeStore.Core.Models;
using BikeStore.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BikeStore.DAL.Repositories
{
    public class BrandRepository : Repository<Brand>, IBrandRepository
    {
        private BikeStoreDBContext BikeStoreDBContext
        {
            get { return Context as BikeStoreDBContext; }
        }

        public BrandRepository(BikeStoreDBContext context)
            : base(context)
        { }

        public async Task<IEnumerable<Brand>> GetAllWithBikesAsync()
        {
            return await BikeStoreDBContext.Brands
                .Include(b => b.Bikes)
                .ToListAsync();
        }

        public async Task<Brand> GetWithBikesByIdAsync(Guid id)
        {
            return await BikeStoreDBContext.Brands
                .Include(b => b.Bikes)
                .SingleOrDefaultAsync(b => b.Id == id);
        }
    }
}
