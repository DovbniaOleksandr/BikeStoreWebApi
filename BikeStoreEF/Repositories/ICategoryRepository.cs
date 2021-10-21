using BikeStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeStoreEF.Repositories
{
    public interface ICategoryRepository: IRepository<Category>
    {
        Task<IEnumerable<Category>> GetAllWithBikesAsync();
        Task<Category> GetWithBikesByIdAsync(int id);
    }
}
