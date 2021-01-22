﻿using Faces.WebMvc.ViewModels;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Faces.WebMvc.RestClients
{
    interface IOrderManagementApi
    {
        [Get("/orders")]
        Task<List<OrderViewModel>> GetOrders();

        [Get("/orders/{id}")]
        Task<OrderViewModel> GetOrderById(Guid orderId);
    }
}
