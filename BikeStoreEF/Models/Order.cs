using System;
using System.Collections.Generic;
using System.Text;

namespace BikeStore.Core.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid BikeId { get; set; }
        public Bike Bike { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsCompleted { get; set; }
    }
}
