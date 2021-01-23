namespace Faces.Shared.Messaging.InterfacesConstants
{
    public class RabbitmqMassTransitConstants
    {
        public const string RabbitmqUri = "rabbitmq://rabbitmq:5672/";
        public const string UserName = "user";
        public const string Password = "bX1DTrlOfH";
        public const string RegisterOrderCommandQueue = "register.order.command";
        public const string NotificationServiceQueue = "notification.service.queue";
        public const string OrderDispatchedServiceQueue = "order.dispatch.service.queue";
    }
}
