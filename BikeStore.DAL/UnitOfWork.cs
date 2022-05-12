using BikeStore.Core.Repositories;
using BikeStore.DAL.Repositories;
using BikeStoreEF;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BikeStore.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BikeStoreDBContext _context;

        private BikeRepository _bikeRepository;
        private CategoryRepository _categoryRepository;
        private BrandRepository _brandRepository;
        private OrderRepository _orderRepository;

        public UnitOfWork(BikeStoreDBContext context)
        {
            _context = context;
        }

        public IBikeRepository Bikes => _bikeRepository ?? new BikeRepository(_context);

        public ICategoryRepository Categories => _categoryRepository ?? new CategoryRepository(_context);

        public IBrandRepository Brands => _brandRepository ?? new BrandRepository(_context);

        public IOrderRepository Orders => _orderRepository ?? new OrderRepository(_context);

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
