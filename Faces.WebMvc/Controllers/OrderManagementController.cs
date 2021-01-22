using Faces.WebMvc.RestClients;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Faces.WebMvc.Controllers
{
    public class OrderManagementController : Controller
    {
        private readonly IOrderManagementApi _orderManagementApi;

        public OrderManagementController(IOrderManagementApi orderManagementApi)
        {
            _orderManagementApi = orderManagementApi;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _orderManagementApi.GetOrders();
            foreach (var order in orders)
            {
                order.ImageString = ConvertAndFormatToString(order.ImageData);
            }
            return View(orders);
        }

        //TODO: private methods in controllers are not allowed.
        private string ConvertAndFormatToString(byte[] bytes)
        {
            return $"data:image/png;base64, {Convert.ToBase64String(bytes)}";
        }
    }
}
