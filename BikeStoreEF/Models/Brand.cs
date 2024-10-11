using System;
using System.Collections.Generic;

namespace BikeStore.Core.Models
{
    public class Brand
    {
        public Guid Id { get; set; }
        public string BrandName { get; set; }
        public IEnumerable<Bike> Bikes { get; set; }
    }
}