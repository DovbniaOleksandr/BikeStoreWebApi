using BikeStore.Core.Models;
using BikeStoreWebApi.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BikeStore.Core.Services
{
    public interface IBikeService
    {
        Task<BikeDto> GetBikeById(int id);
        IEnumerable<BikeDto> GetBikesByBrand(string brand);
        IEnumerable<BikeDto> GetBikesByCategory(string category);
        Task<IEnumerable<BikeDto>> GetAllBikes();
        Task<IEnumerable<BikeDto>> GetAllBikesWithCategoryAndBrand();
        Task<BikeDto> GetBikeWithCategoryAndBrand(int id);
        Task<BikeDto> CreateBike(SaveBikeDto newBike);
        Task<bool> UpdateBike(int id, SaveBikeDto bike);
        Task DeleteBike(Bike bike);
        Task<IEnumerable<BikeDto>> FilterBikes(BikeFilters filters);
    }
}
