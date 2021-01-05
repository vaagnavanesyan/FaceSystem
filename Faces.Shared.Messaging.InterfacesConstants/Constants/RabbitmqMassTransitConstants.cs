namespace Faces.Shared.Messaging.InterfacesConstants
{
    public class RabbitmqMassTransitConstants
    {
        public const string RabbitmqUri = "rabbitmq://rabbitmq:5672/";
        public const string UserName = "guest";
        public const string Password = "guest";
        public const string RegisterOrderCommandQueue = "register.order.command";
    }
}
