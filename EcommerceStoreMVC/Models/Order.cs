﻿using Microsoft.EntityFrameworkCore;

namespace EcommerceStoreMVC.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string CustomerId { get; set; } = "";
        public ApplicationUser Customer { get; set; } = null!;

        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        [Precision(16, 2)]
        public decimal ShippingFee { get; set; }

        public string DeliveryAddress { get; set; } = "";
        public string PaymentMethod { get; set; } = "";
        public string PaymentStatus { get; set; } = "";
        public string PaymentDetails { get; set; } = ""; // to store paypal details
        public string OrderStatus { get; set; } = "";
        public DateTime CreatedAt { get; set; }
    }
}
