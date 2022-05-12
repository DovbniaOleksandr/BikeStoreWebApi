using BikeStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BikeStore.Core.Services
{
    public interface IBikeService
    {
        Task<Bike> GetBikeById(int id);
        IEnumerable<Bike> GetBikesByBrand(string brand);
        IEnumerable<Bike> GetBikesByCategory(string category);
        Task<IEnumerable<Bike>> GetAllBikes();
        Task<IEnumerable<Bike>> GetAllBikesWithCategoryAndBrand();
        Task<Bike> GetBikeWithCategoryAndBrand(int id);
        Task<Bike> CreateBike(Bike newBike);
        Task<bool> UpdateBike(int id, Bike bike);
        Task DeleteBike(Bike bike);
        Task<IEnumerable<Bike>> FilterBikes(BikeFilters filters);
    }
}
