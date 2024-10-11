using BikeStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BikeStore.Core.Repositories
{
    public interface IBikeRepository: IRepository<Bike>
    {
        Task<IEnumerable<Bike>> GetAllWithBrandAndCategoryAsync();
        Task<Bike> GetWithBrandAndCategoryByIdAsync(Guid id);
        Task<IEnumerable<Bike>> FindAllWithBrandAndCategoryAsync(Expression<Func<Bike, bool>> predicate);
    }
}
