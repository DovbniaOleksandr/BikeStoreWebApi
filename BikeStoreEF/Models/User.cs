using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BikeStore.Core.Models
{
    public class User: IdentityUser<Guid>
    {
        public List<Order> Orders { get; set; }
    }
}
