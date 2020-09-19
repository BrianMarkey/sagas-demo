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
using Rebus.Bus;
using Rebus.Config;
using Shared.Messages;
using Shared.Modules;
using Shared.Routing;

namespace AdeptusAdministratum
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
        }

        public virtual void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterHandler<ComplianceSaga>();

            var routes = new List<BusRoute>
            {
                new BusRoute
                {
                    Bus = Buses.AdeptusAstartes,
                    Type = typeof(AssignLegionMessage)
                },
                new BusRoute
                {
                    Bus = Buses.AdeptusAstartes,
                    Type = typeof(InitiateReconMessage)
                },
                new BusRoute
                {
                    Bus = Buses.ImperialGuard,
                    Type = typeof(CreateImperialGuardDeploymentMessage)
                },
                new BusRoute
                {
                    Bus = Buses.ImperialGuard,
                    Type = typeof(CreateImperialGuardReinforcementMessage)
                }
            };

            builder.RegisterModule(new RebusModule(_configuration, Buses.AdeptusAdministratum, new List<BusRoute>(routes)));
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
                endpoints.MapPost("/complianceorder", async context =>
                {
                    var bus = app.ApplicationServices.GetService<IBus>();

                    var world = context.Request.Form["world"];

                    await bus.SendLocal(new ComplianceOrderInitiationMessage
                    {
                        ID = Guid.NewGuid(),
                        WorldName = world
                    });

                    context.Response.Redirect("/");
                });
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
