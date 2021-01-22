using Faces.WebMvc.RestClients;
using Microsoft.AspNetCore.Mvc;
using System;
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

        [Route("/Details/{orderId}")]
        public async Task<IActionResult> Details(Guid orderId)
        {
            var order = await _orderManagementApi.GetOrderById(orderId);
            order.ImageString = ConvertAndFormatToString(order.ImageData);

            foreach (var detail in order.OrderDetails)
            {
                detail.ImageString = ConvertAndFormatToString(detail.FaceData);
            }

            return View(order);
        }

        //TODO: private methods in controllers are not allowed.
        private string ConvertAndFormatToString(byte[] bytes)
        {
            return $"data:image/png;base64, {Convert.ToBase64String(bytes)}";
        }
    }
}
