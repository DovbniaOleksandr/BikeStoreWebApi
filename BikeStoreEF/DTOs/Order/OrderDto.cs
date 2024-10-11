using BikeStoreWebApi.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeStoreWebApi.DTOs.Order
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public UserDto User { get; set; }
        public BikeDto Bike { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsCompleted { get; set; }
    }
}
