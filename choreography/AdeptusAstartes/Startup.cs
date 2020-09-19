using AdeptusAstartes.Data;
using AdeptusAstartes.MessageHandlers;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Rebus.Bus;
using Rebus.Config;
using Shared.Messages;
using Shared.Modules;
using Shared.Routing;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdeptusAstartes.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace AdeptusAstartes
{
    public class Startup
    {
        readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
        }

        public virtual void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterInstance(new DB()).AsSelf().SingleInstance();

            builder.RegisterHandler<PingMessageHandler>();
            builder.RegisterHandler<ComplianceStatusChangeMessageHandler>();
            builder.RegisterHandler<ComplianceOrderCommandHandler>();
            builder.RegisterHandler<ImperialGuardDeploymentStatusMessageHandler>();
            builder.RegisterHandler<ReconAssignedMessageHandler>();

            var busRoutes = new List<BusRoute>
            {
                // Send the compliance order acknowledged message
                // directly to the AdeptusAdministratum bus
                new BusRoute
                {
                    Bus = Buses.AdeptusAdministratum,
                    Type = typeof(ComplianceOrderAcknowledgedMessage)
                }
            };

            builder.RegisterModule(new RebusModule(_configuration, Buses.AdeptusAstartes, busRoutes));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<AstartesHub>("/astarteshub");

                endpoints.MapGet("/complianceorders", async content =>
                {
                    var db = content.RequestServices.GetRequiredService<DB>();

                    var orders = db.ComplianceOrders.Where(o => o.Status != ComplianceStatus.Complete);

                    await content.Response.WriteAsync(JsonConvert.SerializeObject(orders));
                });
            });

            subscribeToMessages(app);
        }

        private void subscribeToMessages(IApplicationBuilder app)
        {
            var bus = app.ApplicationServices.GetService<IBus>();

            Task.WaitAll(
                bus.Subscribe<PingMessage>(),
                bus.Subscribe<ImperialGuardDeploymentStatusMessage>(),
                bus.Subscribe<ComplianceStatusChangeMessage>(),
                bus.Subscribe<ReconAssignedMessage>()
            );
        }
    }
}
