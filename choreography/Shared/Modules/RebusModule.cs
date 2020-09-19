using Autofac;
using Microsoft.Extensions.Configuration;
using Shared.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Modules
{
    public class RebusModule : Module
    {
        readonly IConfiguration _configuration;
        readonly List<BusRoute> _busRoutes;
        readonly Buses _thisBus;

        public RebusModule(IConfiguration configuration, Buses thisBus, List<BusRoute> busRoutes)
        {
            _configuration = configuration;
            _thisBus = thisBus;
            _busRoutes = busRoutes;
        }

        protected override void Load(ContainerBuilder builder)
        {
            switch (_configuration["transport"])
            {
                case "rabbitmq":
                    builder.RegisterModule(new RebusRabbitModule(_configuration, _thisBus, _busRoutes));
                    break;

                default:
                    builder.RegisterModule(new RebusSqlModule(_configuration, _thisBus, _busRoutes));
                    break;
            }
        }
    }
}
