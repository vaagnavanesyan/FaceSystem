﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
﻿namespace Messaging.InterfacesConstants.Constants
{
    public class RabbitmqMassTransitConstants
    {
        public const string RabbitmqUri = "rabbitmq://rabbitmq:5672/";
        public const string UserName = "guest";
        public const string Password = "guest";
        public const string RegisterOrderCommandQueue = "register.order.command";
    }
}
