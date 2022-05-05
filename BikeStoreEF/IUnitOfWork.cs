using BikeStore.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeStoreEF
{
    public interface IUnitOfWork: IDisposable
    {
        IBikeRepository Bikes { get; }
        ICategoryRepository Categories { get; }
        IBrandRepository Brands { get; }
        IUserRepository Users { get; }
        IRoleRepository Roles { get; }

        Task<int> SaveAsync();
    }
}
