using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Messages
{
    public class ComplianceOrderCommand
    {
        public Guid ID { get; set; }
        public string WorldName { get; set; }
    }
}
