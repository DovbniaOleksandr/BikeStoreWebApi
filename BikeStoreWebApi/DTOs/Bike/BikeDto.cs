using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeStoreWebApi.DTOs
{
    public class BikeDto
    {
        public int BikeId { get; set; }
        public string Name { get; set; }
        public short ModelYear { get; set; }
        public decimal Price { get; set; }
        public string BikePhoto { get; set; }
        public string Description { get; set; }

        public BrandDto Brand { get; set; }
        public CategoryDto Category { get; set; }
    }
}
