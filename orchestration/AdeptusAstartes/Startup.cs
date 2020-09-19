
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
using Microsoft.AspNetCore.SignalR;
using AdeptusAstartes.MessageHandlers;
using AdeptusAstartes.Data;

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

            builder.RegisterHandler<AssignLegionMessageHandler>();
            builder.RegisterHandler<InitiateReconMessageHandler>();
            builder.RegisterHandler<ReconInitiatedMessageHandler>();

            var busRoutes = new List<BusRoute>
            {
                new BusRoute
                {
                    Bus = Buses.AdeptusAdministratum,
                    Type = typeof(LegionAssignedMessage)
                },
                new BusRoute
                {
                    Bus = Buses.AdeptusAdministratum,
                    Type = typeof(ReconInitiatedMessage)
                },
                new BusRoute
                {
                    Bus = Buses.AdeptusAdministratum,
                    Type = typeof(ReconCompletedMessage)
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

            });

            subscribeToMessages(app);
        }

        private void subscribeToMessages(IApplicationBuilder app)
        {
            var bus = app.ApplicationServices.GetService<IBus>();

            Task.WaitAll(
            );
        }
    }
}
