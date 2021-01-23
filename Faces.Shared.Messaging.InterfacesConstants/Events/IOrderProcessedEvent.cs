﻿using System;
using System.Collections.Generic;

namespace Faces.Shared.Messaging.InterfacesConstants.Events
{
    public interface IOrderProcessedEvent
    {
        Guid OrderId { get; }
        string PictureUrl { get; }
        List<byte[]> Faces { get; }
        string UserEmail { get; }
    }

}
