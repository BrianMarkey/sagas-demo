using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Messages
{
    public class ComplianceOrderInitiationMessage
    {
        public Guid ID { get; set; }
        public string WorldName { get; set; }
    }
}
