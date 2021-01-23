using System;

namespace Faces.Shared.Messaging.InterfacesConstants.Events
{
    public interface IOrderRegisteredEvent
    {
        Guid OrderId { get; }
    }

}
