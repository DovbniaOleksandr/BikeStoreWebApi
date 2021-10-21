using BikeStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BikeStoreEF.Repositories
{
    public interface IBikeRepository: IRepository<Bike>
    {
        Task<IEnumerable<Bike>> GetAllWithBrandAndCategoryAsync();
        Task<Bike> GetWithBrandAndCategoryByIdAsync(int id);
        Task<IEnumerable<Bike>> FindAllWithBrandAndCategory(Expression<Func<Bike, bool>> predicate);
    }
}
