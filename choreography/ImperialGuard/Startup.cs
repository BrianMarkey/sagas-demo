using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using ImperialGuard.MessageHandlers;
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
using ImperialGuard.Data;

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
            builder.RegisterInstance(new DB()).AsSelf().SingleInstance();

            builder.RegisterHandler<PingMessageHandler>();
            builder.RegisterHandler<ReconCompletedMessageHandler>();
            builder.RegisterHandler<ImperialGuardDeploymentStatusMessageHandler>();
            builder.RegisterHandler<ComplianceStatusChangeMessageHandler>();
            builder.RegisterHandler<ComplianceResourcesDepletedMessageHandler>();

            builder.RegisterModule(new RebusModule(_configuration, Buses.ImperialGuard, new List<BusRoute>()));
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
                bus.Subscribe<PingMessage>(),
                bus.Subscribe<ComplianceStatusChangeMessage>(),
                bus.Subscribe<ImperialGuardDeploymentStatusMessage>(),
                bus.Subscribe<ComplianceResourcesDepletedMessage>(),
                bus.Subscribe<ReconCompletedMessage>()
            );
        }
    }
}
