using Faces.WebMvc.ViewModels;
using Microsoft.Extensions.Configuration;
using Refit;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Faces.WebMvc.RestClients
{
    public class OrderManagementApi : IOrderManagementApi
    {
        private IOrderManagementApi _restClient;

        public OrderManagementApi(IConfiguration configuration, HttpClient httpClient)
        {
            var ordersApiLocation = configuration.GetSection("ApiServiceLocations").GetValue<string>("OrdersApi");
            httpClient.BaseAddress = new Uri($"http://{ordersApiLocation}/api");
            _restClient = RestService.For<IOrderManagementApi>(httpClient);
        }

        public Task<OrderViewModel> GetOrderById(Guid orderId)
        {
            try
            {
                return _restClient.GetOrderById(orderId);
            }
            catch (ApiException ex)
            {

                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                throw;
            }
        }

        public Task<List<OrderViewModel>> GetOrders()
        {
            return _restClient.GetOrders();
        }
    }
}
