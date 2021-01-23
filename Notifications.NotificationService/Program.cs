using Faces.Shared.Messaging.InterfacesConstants;
using GreenPipes;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Notifications.EmailService;
using Notifications.NotificationService.Consumers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Notifications.NotificationService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                var emailConfig = hostContext.Configuration.GetSection("EmailConfiguration").Get<EmailConfig>();
                services.AddSingleton(emailConfig);
                services.AddScoped<IEmailSender, EmailSender>();

                services.AddMassTransit(x =>
                {
                    x.AddConsumer<OrderProcessedEventConsumer>();

                    x.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.Host("localhost", "/", host =>
                        {
                            host.Username(RabbitmqMassTransitConstants.UserName);
                            host.Password(RabbitmqMassTransitConstants.Password);
                        });

                        cfg.ReceiveEndpoint(RabbitmqMassTransitConstants.NotificationServiceQueue, e =>
                        {
                            e.PrefetchCount = 16;
                            e.UseMessageRetry(x => x.Interval(2, TimeSpan.FromSeconds(10)));
                            e.ConfigureConsumer<OrderProcessedEventConsumer>(context);
                        });
                    });
                });

                services.AddMassTransitHostedService();

                services.AddHostedService<Worker>();
            });
    }
}
