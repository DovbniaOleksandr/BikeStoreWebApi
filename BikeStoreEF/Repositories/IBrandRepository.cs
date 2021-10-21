using BikeStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeStoreEF.Repositories
{
    public interface IBrandRepository: IRepository<Brand>
    {
        Task<IEnumerable<Brand>> GetAllWithBikesAsync();
        Task<Brand> GetWithBikesByIdAsync(int id);
    }
}
