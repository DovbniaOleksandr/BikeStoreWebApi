using BikeStore.Core.Models;
using BikeStore.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BikeStore.DAL.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        private BikeStoreDBContext BikeStoreDBContext
        {
            get { return Context as BikeStoreDBContext; }
        }

        public RoleRepository(BikeStoreDBContext context)
            : base(context)
        { }
    }
}
