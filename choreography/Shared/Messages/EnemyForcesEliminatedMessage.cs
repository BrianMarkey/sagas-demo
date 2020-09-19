using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Messages
{
    public class EnemyForcesEliminatedMessage
    {
        public Guid ComplianceOrderID { get; set; }
        public string WorldName { get; set; }
    }
}
