using Faces.Shared.Messaging.InterfacesConstants.Events;
using MassTransit;
using Ordering.OrdersApi.Models;
using Ordering.OrdersApi.Persistence;
using System;
using System.Threading.Tasks;

namespace Ordering.OrdersApi.Messages.Consumers
{
    public class OrderDispatchedEventConsumer : IConsumer<IOrderDispatchedEvent>
    {
        private readonly IOrderRepository _orderRepository;
        public OrderDispatchedEventConsumer(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public Task Consume(ConsumeContext<IOrderDispatchedEvent> context)
        {
            var message = context.Message;
            Guid orderId = message.OrderId;
            UpdateDatabase(orderId);
            return Task.CompletedTask;
        }

        private void UpdateDatabase(Guid orderId)
        {
            var order = _orderRepository.GetOrder(orderId);
            if (order != null)
            {
                order.Status = Status.Sent;
                _orderRepository.UpdateOrder(order);
            }
        }
    }

}
