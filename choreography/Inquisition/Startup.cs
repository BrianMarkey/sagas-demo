using System;
using System.Linq;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rebus.Bus;
using Shared.Messages;
using Web.MessageHandlers;
using Web.SignalR;
using Rebus.Config;
using Shared.Modules;
using System.Threading.Tasks;
using Shared.Routing;
using System.Collections.Generic;
using Inquisition.MessageHandlers;

namespace demo
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
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ActivityHub>("/activityhub");

                endpoints.MapGet("/send", async content =>
                {
                    var bus = content.RequestServices.GetRequiredService<IBus>();

                    await bus.Publish(new PingMessage
                    {
                        Message = content.Request.Query["message"]
                    });

                    await content.Response.WriteAsync("Okay done");
                });
            });

            subscribeToMessages(app);
        }

        public virtual void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<ActivityHub>().AsSelf().InstancePerLifetimeScope();

            builder.RegisterHandler<PingMessageHandler>();
            builder.RegisterHandler<RemembrancerReportMessageHandler>();

            builder.RegisterModule(new RebusModule(_configuration, Buses.Inquisition, new List<BusRoute>()));
        }

        private void subscribeToMessages(IApplicationBuilder app)
        {
            var bus = app.ApplicationServices.GetService<IBus>();
            bus.Subscribe<RemembrancerReportMessage>().Wait();
        }
    }
}
