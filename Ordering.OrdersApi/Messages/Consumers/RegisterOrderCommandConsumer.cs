using Faces.Shared.Messaging.InterfacesConstants;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.OrdersApi.Messages.Consumers
{
    public class RegisterOrderCommandConsumer : IConsumer<IRegisterOrderCommand>
    {
        private readonly ILogger<RegisterOrderCommandConsumer> _logger;

        public RegisterOrderCommandConsumer(ILogger<RegisterOrderCommandConsumer> logger)
        {
            this._logger = logger;
        }
        public Task Consume(ConsumeContext<IRegisterOrderCommand> context)
        {
            return Task.Run(() => _logger.LogInformation("PictureUri: {PictureUri}", context.Message.PictureUri));
        }
    }
}
