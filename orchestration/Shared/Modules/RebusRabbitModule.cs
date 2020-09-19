using Autofac;
using Microsoft.Extensions.Configuration;
using Rebus.Config;
using Rebus.Retry.Simple;
using Rebus.Routing.TypeBased;
using Shared.Routing;
using System.Collections.Generic;

namespace Shared.Modules
{
    public class RebusRabbitModule : Module
    {
        private readonly IConfiguration _configuration;
        readonly List<BusRoute> _busRoutes;
        readonly Buses _thisBus;

        public RebusRabbitModule(IConfiguration configuration, Buses thisBus, List<BusRoute> busRoutes)
        {
            _configuration = configuration;
            _thisBus = thisBus;
            _busRoutes = busRoutes;
        }

        protected override void Load(ContainerBuilder builder)
        {
            registerRebus(builder);
        }

        private void registerRebus(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterRebus((configurer, context) =>
            {
                configureRabbtMQServerTransport(configurer);

                configurer.Options(o =>
                {
                    o.SetNumberOfWorkers(3);
                    o.SetMaxParallelism(3);
                    o.SimpleRetryStrategy(
                        maxDeliveryAttempts: 1,
                        errorQueueAddress: "rbs.Rebus_Error",
                        secondLevelRetriesEnabled: true);
                });

                configurer.Routing(r =>
                {
                    foreach (var bus in _busRoutes)
                    {
                        r.TypeBased().Map(bus.Type, bus.Bus.ToString());
                    }
                });


                return configurer;
            });
        }

        private void configureRabbtMQServerTransport(RebusConfigurer configurer)
        {
            var sqlConnectionString = _configuration.GetConnectionString("RebusSql");

            var rabbitConnectionString = _configuration.GetConnectionString("RebusRabbit");

            configurer.Transport(t => t.UseRabbitMq(rabbitConnectionString, _thisBus.ToString()))
                .Sagas(x =>
                {
                    x.StoreInSqlServer(
                        connectionString: sqlConnectionString,
                        dataTableName: "rbs.Rebus_Sagas",
                        indexTableName: "rbs.Rebus_SagasIndex");
                });
        }
    }
}
