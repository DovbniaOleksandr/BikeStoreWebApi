using System;
using System.Collections.Generic;
using System.Text;

namespace BikeStore.Core.Models
{
    public class BikeFilters
    {
        public string Name { get; set; }
        public int? ModelYear { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public List<Guid> Categories { get; set; }
        public List<Guid> Brands { get; set; }
    }
}
