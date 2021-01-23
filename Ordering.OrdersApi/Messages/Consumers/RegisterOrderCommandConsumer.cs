using Faces.Shared.Messaging.InterfacesConstants;
using Faces.Shared.Messaging.InterfacesConstants.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Ordering.OrdersApi.Models;
using Ordering.OrdersApi.Persistence;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ordering.OrdersApi.Messages.Consumers
{
    public class RegisterOrderCommandConsumer : IConsumer<IRegisterOrderCommand>
    {
        private readonly ILogger<RegisterOrderCommandConsumer> _logger;
        private readonly IOrderRepository _orderRepository;
        private readonly IHttpClientFactory _clientFactory;

        public RegisterOrderCommandConsumer(ILogger<RegisterOrderCommandConsumer> logger, IOrderRepository orderRepository, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _orderRepository = orderRepository;
            _clientFactory = clientFactory;
        }
        public async Task Consume(ConsumeContext<IRegisterOrderCommand> context)
        {
            var order = context.Message;
            await SaveOrder(order);
            var client = _clientFactory.CreateClient();
            Dictionary<Guid, List<byte[]>> orderDetails = await GetFacesFromFacesApiAsync(client, order.ImageData, order.OrderId);
            var faces = orderDetails[order.OrderId];
            await SaveOrderDetails(order.OrderId, faces);
            await context.Publish<IOrderProcessedEvent>(new
            {
                OrderId = order.OrderId,
                order.UserEmail,
                Faces = faces,
                order.PictureUri
            });
        }

        private async Task SaveOrder(IRegisterOrderCommand command)
        {
            Order order = new Order
            {
                ImageData = command.ImageData,
                OrderId = command.OrderId,
                PictureUri = command.PictureUri,
                UserEmail = command.UserEmail,
            };

            await _orderRepository.RegisterOrder(order);
        }

        private async Task<Dictionary<Guid, List<byte[]>>> GetFacesFromFacesApiAsync(HttpClient httpClient, byte[] imageData, Guid orderId)
        {
            var byteContent = new ByteArrayContent(imageData);
            byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            var urlAddress = @$"http://localhost:5600/api/faces/{orderId}"; //TODO: pass url and use refit as option
            using (var response = await httpClient.PostAsync(urlAddress, byteContent))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<Dictionary<Guid, List<byte[]>>>(apiResponse);
                return result;
            }
        }

        private async Task SaveOrderDetails(Guid orderId, List<byte[]> faces)
        {
            var order = await _orderRepository.GetOrderAsync(orderId);
            if (order == null)
            {
                return; //TODO: this is strange situation which should be logged and metric should be emitted.
            }

            foreach (var face in faces)
            {
                order.OrderDetails.Add(
                    new OrderDetail
                    {
                        OrderId = orderId,
                        FaceData = face
                    });
            }
            order.Status = Status.Processed;
            _orderRepository.UpdateOrder(order);
        }
    }
}
