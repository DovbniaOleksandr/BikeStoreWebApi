using BikeStore.Core.Models;
using BikeStore.Core.Services;
using BikeStoreEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeStore.Services
{
    public class BikeService : IBikeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BikeService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<Bike> CreateBike(Bike newBike)
        {
            await _unitOfWork.Bikes.AddAsync(newBike);
            await _unitOfWork.SaveAsync();

            return newBike;
        }

        public async Task DeleteBike(Bike bike)
        {
            _unitOfWork.Bikes.Remove(bike);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<Bike>> FilterBikes(BikeFilters filters)
        {
            return await _unitOfWork.Bikes
                .FindAllWithBrandAndCategoryAsync(b => 
                (filters.Categories.Contains(b.CategoryId) || filters.Categories == null) &&
                (filters.Brands.Contains(b.BrandId) || filters.Brands == null) &&
                (b.Name.Contains(filters.Name) || filters.Name == null) &&
                (b.ModelYear == filters.ModelYear || filters.ModelYear == null) &&
                (b.Price >= filters.MinPrice || filters.MinPrice == null) &&
                (b.Price <= filters.MaxPrice || filters.MaxPrice == null));
        }

        public async Task<IEnumerable<Bike>> GetAllBikes()
        {
            return await _unitOfWork.Bikes.GetAllAsync();
        }

        public async Task<IEnumerable<Bike>> GetAllBikesWithCategoryAndBrand()
        {
            return await _unitOfWork.Bikes.GetAllWithBrandAndCategoryAsync();
        }

        public async Task<Bike> GetBikeById(int id)
        {
            return await _unitOfWork.Bikes.GetByIdAsync(id);
        }

        public IEnumerable<Bike> GetBikesByBrand(string brand)
        {
            return _unitOfWork.Bikes.Find(x => x.Brand.BrandName == brand);
        }

        public async Task<Bike> GetBikeWithCategoryAndBrand(int id)
        {
            return await _unitOfWork.Bikes.GetWithBrandAndCategoryByIdAsync(id);
        }

        public async Task<bool> UpdateBike(int id, Bike bike)
        {
            if (bike == null)
            {
                throw new ArgumentNullException();
            }

            var bikeToBeUpdated = await _unitOfWork.Bikes.GetWithBrandAndCategoryByIdAsync(id);

            if(bikeToBeUpdated == null)
            {
                return false;
            }

            bikeToBeUpdated.Name = bike.Name;
            bikeToBeUpdated.BrandId = bike.BrandId;
            bikeToBeUpdated.CategoryId = bike.CategoryId;
            bikeToBeUpdated.Price = bike.Price;
            bikeToBeUpdated.ModelYear = bike.ModelYear;
            bikeToBeUpdated.BikePhoto = bike.BikePhoto;
            bikeToBeUpdated.Description = bike.Description;

            await _unitOfWork.SaveAsync();

            return true;
        }

        IEnumerable<Bike> IBikeService.GetBikesByCategory(string category)
        {
            return _unitOfWork.Bikes.Find(x => x.Category.Name == category);
        }
    }
}
