using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeStoreWebApi.DTOs
{
    public class SaveBikeDto
    {
        public string Name { get; set; }
        public Guid BrandId { get; set; }
        public Guid CategoryId { get; set; }
        public short ModelYear { get; set; }
        public decimal Price { get; set; }
        public string BikePhoto { get; set; }
        public string Description { get; set; }
    }
}
