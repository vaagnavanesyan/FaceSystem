﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ordering.OrdersApi.Models
{
    public class Order
    {
        public Order()
        {
            OrderDetails = new List<OrderDetail>();
        }
        public Guid OrderId { get; set; }
        public string PictureUri { get; set; }
        public byte[] ImageData { get; set; }
        public string UserEmail { get; set; }
        public Status Status { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        
        [NotMapped]
        public string OrderStatus { get; set; }
    }
}
