using AutoMapper;
using BikeStore.Core.Models;
using BikeStore.Core.Services;
using BikeStoreEF;
using BikeStoreWebApi.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BikeStore.Services
{
    public class BrandService : IBrandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BrandService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BrandDto> CreateBrand(SaveBrandDto newBrand)
        {
            var brandToCreate = _mapper.Map<SaveBrandDto, Brand>(newBrand);

            await _unitOfWork.Brands.AddAsync(brandToCreate);
            await _unitOfWork.SaveAsync();

            var brandDto = _mapper.Map<Brand, BrandDto>(brandToCreate);

            return brandDto;
        }

        public async Task DeleteBrand(BrandDto brand)
        {
            var brandToDelete = _mapper.Map<BrandDto, Brand>(brand);

            _unitOfWork.Brands.Remove(brandToDelete);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<BrandDto>> GetAllBrands()
        {
            var brands = await _unitOfWork.Brands.GetAllAsync();

            return _mapper.Map<IEnumerable<Brand>, IEnumerable<BrandDto>>(brands);
        }

        public async Task<BrandDto> GetBrandById(int id)
        {
            var brand = await _unitOfWork.Brands.GetByIdAsync(id);

            return _mapper.Map<Brand, BrandDto>(brand);
        }

        public async Task<bool> UpdateBrand(int id, SaveBrandDto brand)
        {
            if (brand == null)
            {
                throw new ArgumentNullException();
            }

            var brandToBeUpdated = await _unitOfWork.Brands.GetWithBikesByIdAsync(id);

            if (brandToBeUpdated == null)
            {
                return false;
            }

            brandToBeUpdated.BrandName = brand.BrandName;

            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
