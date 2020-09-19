using Shared.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Messages
{
    public class RemembrancerReportMessage
    {
        public Buses Origin { get; set; }
        public string Message { get; set; }
    }
}
