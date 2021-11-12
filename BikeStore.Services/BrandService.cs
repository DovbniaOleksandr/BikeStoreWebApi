using BikeStore.Core.Models;
using BikeStore.Core.Services;
using BikeStoreEF;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BikeStore.Services
{
    public class BrandService : IBrandService
    {
        private readonly IUnitOfWork _unitOfWork;
        public BrandService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<Brand> CreateBrand(Brand newBrand)
        {
            await _unitOfWork.Brands.AddAsync(newBrand);
            await _unitOfWork.SaveAsync();
            return newBrand;
        }

        public async Task DeleteBrand(Brand brand)
        {
            _unitOfWork.Brands.Remove(brand);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<Brand>> GetAllBrands()
        {
            return await _unitOfWork.Brands.GetAllAsync();
        }

        public async Task<Brand> GetBrandById(int id)
        {
            return await _unitOfWork.Brands.GetByIdAsync(id);
        }

        public async Task UpdateBrand(Brand brandToBeUpdated, Brand brand)
        {
            brandToBeUpdated.BrandId = brand.BrandId;
            brandToBeUpdated.BrandName = brand.BrandName;

            await _unitOfWork.SaveAsync();
        }
    }
}
