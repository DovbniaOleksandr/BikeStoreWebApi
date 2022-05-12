using System;
using System.Collections.Generic;
using System.Text;

namespace BikeStore.Core.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int BikeId { get; set; }
        public Bike Bike { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsCompleted { get; set; }
    }
}
