using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeStoreWebApi.DTOs.Order
{
    public class SaveOrderDto
    {
        public Guid UserId { get; set; }
        public Guid BikeId { get; set; }
    }
}
