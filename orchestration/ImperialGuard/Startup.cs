using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Modules;
using Shared.Routing;
using Rebus.Config;
using Rebus.Bus;
using Shared.Messages;
using ImperialGuard.MessageHandlers;

namespace ImperialGuard
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
            builder.RegisterHandlersFromAssemblyOf<CreateImperialGuardDeploymentMessageHandler>();

            var routes = new List<BusRoute>
            {
                new BusRoute
                {
                    Bus = Buses.AdeptusAdministratum,
                    Type = typeof(ImperialGuardDeploymentCreatedMessage)
                },
                new BusRoute
                {
                    Bus = Buses.AdeptusAdministratum,
                    Type = typeof(ImperialGuardDeploymentArrivedMessage)
                },
                new BusRoute
                {
                    Bus = Buses.AdeptusAdministratum,
                    Type = typeof(ImperialGuardReinforcementCreatedMessage)
                },
                new BusRoute
                {
                    Bus = Buses.AdeptusAdministratum,
                    Type = typeof(ImperialGuardReinforcementArrivedMessage)
                },
                new BusRoute
                {
                    Bus = Buses.AdeptusAdministratum,
                    Type = typeof(ImperialGuardDeploymentEradicatedMessage)
                },
                new BusRoute
                {
                    Bus = Buses.AdeptusAdministratum,
                    Type = typeof(EnemyForcesEradicatedMessage)
                }
            };

            builder.RegisterModule(new RebusModule(_configuration, Buses.ImperialGuard, routes));
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
