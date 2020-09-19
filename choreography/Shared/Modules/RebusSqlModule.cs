using Autofac;
using Microsoft.Extensions.Configuration;
using Rebus.Config;
using Rebus.Retry.Simple;
using Rebus.Routing.TypeBased;
using Shared.Messages;
using Shared.Routing;
using System.Collections.Generic;

namespace Shared.Modules
{
    public class RebusSqlModule : Module
    {
        readonly IConfiguration _configuration;
        readonly List<BusRoute> _busRoutes;
        readonly Buses _thisBus;

        public RebusSqlModule(IConfiguration configuration, Buses thisBus, List<BusRoute> busRoutes)
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
                configureSqlServerTransport(configurer);

                configurer.Options(o =>
                {
                    o.SetNumberOfWorkers(1);
                    o.SetMaxParallelism(1);
                    o.SimpleRetryStrategy(
                        maxDeliveryAttempts: 1,
                        errorQueueAddress: "rbs.Rebus_Error",
                        secondLevelRetriesEnabled: false);
                });

                configurer.Routing(r =>
                {
                    var routing = r.TypeBased();

                    foreach (var bus in _busRoutes)
                    {
                        routing.Map(bus.Type, getInputQueueName(bus.Bus));
                    }
                });

                return configurer;
            });
        }

        private void configureSqlServerTransport(RebusConfigurer configurer)
        {
            var connectionString = _configuration.GetConnectionString("RebusSql");

            configurer.Transport(t => t.UseSqlServer(
                    transportOptions: new SqlServerTransportOptions(
                    connectionString: connectionString,
                    enlistInAmbientTransaction: false
                ), getInputQueueName(_thisBus)))
                .Subscriptions(x => x.StoreInSqlServer(
                    connectionString: connectionString,
                    tableName: "rbs.Rebus_Subscriptions",
                    isCentralized: true));
        }

        private string getInputQueueName(Buses bus)
        {
            return $"rbs.Rebus_InputQueue_{bus}";
        }
    }
}
