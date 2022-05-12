using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeStoreWebApi.DTOs.Order
{
    public class SaveOrderDto
    {
        public int UserId { get; set; }
        public int BikeId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
