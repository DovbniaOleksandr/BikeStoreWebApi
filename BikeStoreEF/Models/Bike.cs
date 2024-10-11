using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeStore.Core.Models
{
    public class Bike
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid BrandId { get; set; }
        public Guid CategoryId { get; set; }
        public short ModelYear { get; set; }
        public decimal Price { get; set; }
        public string BikePhoto { get; set; }
        public string Description { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual Category Category { get; set; }
    }
}
