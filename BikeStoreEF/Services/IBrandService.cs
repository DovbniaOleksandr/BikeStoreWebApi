using BikeStore.Core.Models;
using BikeStoreWebApi.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BikeStore.Core.Services
{
    public interface IBrandService
    {
        Task<BrandDto> GetBrandById(Guid id);
        Task<IEnumerable<BrandDto>> GetAllBrands();
        Task<BrandDto> CreateBrand(SaveBrandDto newBrand);
        Task<bool> UpdateBrand(Guid id, SaveBrandDto brand);
        Task DeleteBrand(BrandDto brand);
    }
}
