using System;
using System.Collections.Generic;

namespace BikeStore.Core.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public IEnumerable<Bike> Bikes { get; set; }
    }
}