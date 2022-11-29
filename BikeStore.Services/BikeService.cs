using AutoMapper;
using BikeStore.Core.Models;
using BikeStore.Core.Services;
using BikeStoreEF;
using BikeStoreWebApi.DTOs;
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
        private readonly IMapper _mapper;

        public BikeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BikeDto> CreateBike(SaveBikeDto newBike)
        {
            var bikeToCreate = _mapper.Map<SaveBikeDto, Bike>(newBike);

            await _unitOfWork.Bikes.AddAsync(bikeToCreate);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<Bike, BikeDto>(bikeToCreate);
        }

        public async Task DeleteBike(Bike bike)
        {
            var bikeToDelete = await _unitOfWork.Bikes.GetByIdAsync(bike.BikeId);

            _unitOfWork.Bikes.Remove(bikeToDelete);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<BikeDto>> FilterBikes(BikeFilters filters)
        {
            var filteredBikes = await _unitOfWork.Bikes
                .FindAllWithBrandAndCategoryAsync(b => 
                (filters.Categories.Contains(b.CategoryId) || !filters.Categories.Any()) &&
                (filters.Brands.Contains(b.BrandId) || !filters.Brands.Any()) &&
                (b.Name.Contains(filters.Name) || filters.Name == null) &&
                (b.ModelYear == filters.ModelYear || filters.ModelYear == null) &&
                (b.Price >= filters.MinPrice || filters.MinPrice == null) &&
                (b.Price <= filters.MaxPrice || filters.MaxPrice == null));

            return _mapper.Map<IEnumerable<Bike>, IEnumerable<BikeDto>>(filteredBikes);
        }

        public async Task<IEnumerable<BikeDto>> GetAllBikes()
        {
            var bikes = await _unitOfWork.Bikes.GetAllAsync();

            return _mapper.Map<IEnumerable<Bike>, IEnumerable<BikeDto>>(bikes);
        }

        public async Task<IEnumerable<BikeDto>> GetAllBikesWithCategoryAndBrand()
        {
            var bikes = await _unitOfWork.Bikes.GetAllWithBrandAndCategoryAsync();

            return _mapper.Map<IEnumerable<Bike>, IEnumerable<BikeDto>>(bikes);
        }

        public async Task<BikeDto> GetBikeById(int id)
        {
            var bike = await _unitOfWork.Bikes.GetByIdAsync(id);

            return _mapper.Map<Bike, BikeDto>(bike);
        }

        public IEnumerable<BikeDto> GetBikesByBrand(string brand)
        {
            var bikes = _unitOfWork.Bikes.Find(x => x.Brand.BrandName == brand);

            return _mapper.Map<IEnumerable<Bike>, IEnumerable<BikeDto>>(bikes);
        }

        public async Task<BikeDto> GetBikeWithCategoryAndBrand(int id)
        {
            var bike = await _unitOfWork.Bikes.GetWithBrandAndCategoryByIdAsync(id);

            return _mapper.Map<Bike, BikeDto>(bike);
        }

        public async Task<bool> UpdateBike(int id, SaveBikeDto bike)
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

        public IEnumerable<BikeDto> GetBikesByCategory(string category)
        {
            var bikes = _unitOfWork.Bikes.Find(x => x.Category.Name == category);

            return _mapper.Map<IEnumerable<Bike>, IEnumerable<BikeDto>>(bikes);
        }
    }
}
