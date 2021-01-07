using Faces.Shared.Messaging.InterfacesConstants;
using MassTransit;
using Microsoft.Extensions.Logging;
using Ordering.OrdersApi.Models;
using Ordering.OrdersApi.Persistence;
using System;
using System.Threading.Tasks;

namespace Ordering.OrdersApi.Messages.Consumers
{
    public class RegisterOrderCommandConsumer : IConsumer<IRegisterOrderCommand>
    {
        private readonly ILogger<RegisterOrderCommandConsumer> _logger;
        private readonly IOrderRepository _orderRepository;

        public RegisterOrderCommandConsumer(ILogger<RegisterOrderCommandConsumer> logger, IOrderRepository orderRepository)
        {
            _logger = logger;
            _orderRepository = orderRepository;
        }
        public Task Consume(ConsumeContext<IRegisterOrderCommand> context)
        {
            var order = context.Message;
            return SaveOrder(order);
        }

        private Task SaveOrder(IRegisterOrderCommand command)
        {
            Order order = new Order
            {
                ImageData = command.ImageData,
                OrderId = command.OrderId,
                PictureUri = command.PictureUri,
                UserEmail = command.UserEmail,
            };
            
          return _orderRepository.RegisterOrder(order);
        }
    }
}
