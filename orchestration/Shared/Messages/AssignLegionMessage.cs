﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Messages
{
    public class AssignLegionMessage
    {
        public Guid ComplianceOrderID { get; set; }
        public string WorldName { get; set; }
    }
}
