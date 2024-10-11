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
        Task<BikeDto> GetBikeById(Guid id);
        IEnumerable<BikeDto> GetBikesByBrand(string brand);
        IEnumerable<BikeDto> GetBikesByCategory(string category);
        Task<IEnumerable<BikeDto>> GetAllBikes();
        Task<IEnumerable<BikeDto>> GetAllBikesWithCategoryAndBrand();
        Task<BikeDto> GetBikeWithCategoryAndBrand(Guid id);
        Task<BikeDto> CreateBike(SaveBikeDto newBike);
        Task<bool> UpdateBike(Guid id, SaveBikeDto bike);
        Task DeleteBike(Bike bike);
        Task<IEnumerable<BikeDto>> FilterBikes(BikeFilters filters);
    }
}
