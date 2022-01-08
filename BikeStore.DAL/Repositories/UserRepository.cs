using BikeStore.Core.Models;
using BikeStore.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BikeStore.DAL.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private BikeStoreDBContext BikeStoreDBContext
        {
            get { return Context as BikeStoreDBContext; }
        }

        public UserRepository(BikeStoreDBContext context)
            : base(context)
        { }

        public async Task<IEnumerable<User>> GetAllWithRoles()
        {
            return await BikeStoreDBContext.Users
               .Include(u => u.Roles)
               .ToListAsync();
        }

        public async Task<User> GetByEmailAndPasswordWithRoles(string email, string passwd)
        {
            return await BikeStoreDBContext.Users
               .Include(u => u.Roles)
               .SingleOrDefaultAsync(u => u.Email == email && u.Password == passwd);
        }

        public async Task<User> GetByIdWithRoles(int id)
        {
            return await BikeStoreDBContext.Users
               .Include(u => u.Roles)
               .SingleOrDefaultAsync(u => u.Id == id);
        }
    }
}
