using BikeStore.Core.Models;
using BikeStoreEF.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BikeStoreEF.Repositories
{
    public interface IUserRepository: IRepository<User>
    {
        Task<IEnumerable<User>> GetAllWithRoles();
        Task<User> GetByIdWithRoles(int id);
        Task<User> GetByEmailAndPasswordWithRoles(string email, string passwd);
    }
}
