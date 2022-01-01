using BikeStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BikeStore.Core.Services
{
    public interface IBrandService
    {
        Task<Brand> GetBrandById(int id);
        Task<IEnumerable<Brand>> GetAllBrands();
        Task<Brand> CreateBrand(Brand newBrand);
        Task<bool> UpdateBrand(int id, Brand brand);
        Task DeleteBrand(Brand brand);
    }
}
