using Faces.Shared.Messaging.InterfacesConstants;
using GreenPipes;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ordering.OrdersApi.Messages.Consumers;
using Ordering.OrdersApi.Persistence;
using System;

namespace Ordering.OrdersApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<OrdersContext>(options => options.UseSqlServer(Configuration.GetConnectionString("OrdersContextConnectionString")));
            services.AddHttpClient();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddMassTransit(x =>
            {
                x.AddConsumer<RegisterOrderCommandConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", "/", host =>
                    {
                        host.Username(RabbitmqMassTransitConstants.UserName);
                        host.Password(RabbitmqMassTransitConstants.Password);
                    });

                    cfg.ReceiveEndpoint(RabbitmqMassTransitConstants.RegisterOrderCommandQueue, e =>
                    {
                        e.PrefetchCount = 16;
                        e.UseMessageRetry(x => x.Interval(2, TimeSpan.FromSeconds(10)));
                        e.ConfigureConsumer<RegisterOrderCommandConsumer>(context);
                    });
                });
            });

            services.AddMassTransitHostedService();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder
                .AllowAnyMethod()
                .SetIsOriginAllowed(e => true)
                .AllowCredentials()
                .AllowAnyHeader());
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
