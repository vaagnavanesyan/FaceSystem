using System;

namespace Faces.Shared.Messaging.InterfacesConstants.Events
{
    public interface IOrderDispatchedEvent
    {
        Guid OrderId { get; }
        DateTime DispatchDateTime { get; }
    }
}
