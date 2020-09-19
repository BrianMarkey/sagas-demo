using Shared.Messages;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdeptusAstartes.Data
{
    public class DB
    {
        public ConcurrentBag<Legion> Legions { get; set; }
        public ConcurrentBag<ComplianceOrder> ComplianceOrders { get; set; }
        public ConcurrentBag<ImperialGuardDeployment> ImperialGuardDeployments { get; set; }

        public DB()
        {
            Legions = new ConcurrentBag<Legion>
            {
                new Legion
                {
                    Status = LegionStatus.PersuingCompliance,
                    Name = "Ultramarines",
                    Number = 13
                },
                new Legion
                {
                    Status = LegionStatus.PersuingCompliance,
                    Name = "Dark Angels",
                    Number = 1
                },
                new Legion
                {
                    Status = LegionStatus.PersuingCompliance,
                    Name = "Alpha Legion",
                    Number = 20
                },
                new Legion
                {
                    Status = LegionStatus.PersuingCompliance,
                    Name = "Iron Warriors",
                    Number = 4
                },
                new Legion
                {
                    Status = LegionStatus.PersuingCompliance,
                    Name = "Sons of Horus",
                    Number = 16
                },
                new Legion
                {
                    Status = LegionStatus.PersuingCompliance,
                    Name = "World Eaters",
                    Number = 12
                },
                new Legion
                {
                    Status = LegionStatus.PersuingCompliance,
                    Name = "Word Bearers",
                    Number = 17
                },
                new Legion
                {
                    Status = LegionStatus.StandBy,
                    Name = "Blood Angels",
                    Number = 9
                },
                new Legion
                {
                    Status = LegionStatus.StandBy,
                    Name = "Iron Hands",
                    Number = 10
                },
                new Legion
                {
                    Status = LegionStatus.PersuingCompliance,
                    Name = "Emperor's Children",
                    Number = 3
                },
                new Legion
                {
                    Status = LegionStatus.PersuingCompliance,
                    Name = "Imperial Fists",
                    Number = 7
                },
                new Legion
                {
                    Status = LegionStatus.StandBy,
                    Name = "Space Wolves",
                    Number = 6
                },
                new Legion
                {
                    Status = LegionStatus.PersuingCompliance,
                    Name = "Death Guard",
                    Number = 14
                },
                new Legion
                {
                    Status = LegionStatus.StandBy,
                    Name = "Thousand Sons",
                    Number = 15
                },
                new Legion
                {
                    Status = LegionStatus.StandBy,
                    Name = "Salamanders",
                    Number = 18
                },
                new Legion
                {
                    Status = LegionStatus.StandBy,
                    Name = "Raven Guard",
                    Number = 19
                },
                new Legion
                {
                    Status = LegionStatus.PersuingCompliance,
                    Name = "Night Lords",
                    Number = 8
                },
                new Legion
                {
                    Status = LegionStatus.PersuingCompliance,
                    Name = "White Scars",
                    Number = 5
                }
            };

            ComplianceOrders = new ConcurrentBag<ComplianceOrder>();

            ImperialGuardDeployments = new ConcurrentBag<ImperialGuardDeployment>();
        }

        public void StartRecon(string worldName, int legionNumber)
        {
            var legion = Legions.Single(l => l.Number == legionNumber);


        }
    }

    public class Legion
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public LegionStatus Status { get; set; }
    }

    public class ComplianceOrder
    {
        public Guid ID { get; set; }
        public string WorldName { get; set; }
        public ComplianceStatus Status { get; set; }
        public int LegionNumber { get; set; }
    }

    public enum LegionStatus
    {
        StandBy,
        ConductingRecon,
        AwaitingComplianceGoAhead,
        PersuingCompliance
    }

    public class ImperialGuardDeployment
    {
        public Guid ID { get; set; }
        public ImperialGuardDeploymentStatus Status { get; set; }
        public int Tanks { get; set; }
        public int ImperialGuardCompanies { get; set; }
    }
}
