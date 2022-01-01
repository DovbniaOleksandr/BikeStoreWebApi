using System;
using System.Collections.Generic;
using System.Text;

namespace BikeStore.Core.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public IEnumerable<User> Users { get; set; }
    }

    public static class Roles
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }
}
