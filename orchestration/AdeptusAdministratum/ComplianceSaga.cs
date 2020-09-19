using Rebus.Bus;
using Rebus.Handlers;
using Rebus.Sagas;
using Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdeptusAdministratum
{
    public class ComplianceSaga : Saga<ComplianceSagaData>, 
       IAmInitiatedBy<ComplianceOrderInitiationMessage>,
       IHandleMessages<LegionAssignedMessage>,
       IHandleMessages<ReconInitiatedMessage>,
       IHandleMessages<ReconCompletedMessage>,
       IHandleMessages<ImperialGuardDeploymentCreatedMessage>,
       IHandleMessages<ImperialGuardDeploymentArrivedMessage>,
       IHandleMessages<ImperialGuardDeploymentEradicatedMessage>,
       IHandleMessages<ImperialGuardReinforcementCreatedMessage>,
       IHandleMessages<ImperialGuardReinforcementArrivedMessage>,
       IHandleMessages<EnemyForcesEradicatedMessage>
    {
        readonly IBus _bus;

        public ComplianceSaga(IBus bus)
        {
            _bus = bus;
        }

        protected override void CorrelateMessages(ICorrelationConfig<ComplianceSagaData> config)
        {
            config.Correlate<ComplianceOrderInitiationMessage>(x => x.ID, x => x.ComplianceOrderID);
            config.Correlate<LegionAssignedMessage>(x => x.ComplianceOrderID, x => x.ComplianceOrderID);
            config.Correlate<ReconInitiatedMessage>(x => x.ComplianceOrderID, x => x.ComplianceOrderID);
            config.Correlate<ReconCompletedMessage>(x => x.ComplianceOrderID, x => x.ComplianceOrderID);
            config.Correlate<ImperialGuardDeploymentCreatedMessage>(x => x.ComplianceOrderID, x => x.ComplianceOrderID);
            config.Correlate<ImperialGuardDeploymentArrivedMessage>(x => x.ComplianceOrderID, x => x.ComplianceOrderID);
            config.Correlate<ImperialGuardDeploymentEradicatedMessage>(x => x.ComplianceOrderID, x => x.ComplianceOrderID);
            config.Correlate<ImperialGuardReinforcementCreatedMessage>(x => x.ComplianceOrderID, x => x.ComplianceOrderID);
            config.Correlate<ImperialGuardReinforcementArrivedMessage>(x => x.ComplianceOrderID, x => x.ComplianceOrderID);
            config.Correlate<EnemyForcesEradicatedMessage>(x => x.ComplianceOrderID, x => x.ComplianceOrderID);
        }

        public async Task Handle(ComplianceOrderInitiationMessage message)
        {
            Data.ComplianceOrderID = message.ID;
            Data.WorldName = message.WorldName;

            await _bus.Publish(new RemembrancerReportMessage
            {
                Origin = Shared.Routing.Buses.AdeptusAdministratum,
                Message = $"Compliance initiated."
            });

            await _bus.Send(new AssignLegionMessage
            {
                ComplianceOrderID = Data.ComplianceOrderID,
                WorldName = message.WorldName
            });
        }

        public async Task Handle(LegionAssignedMessage message)
        {
            Data.AssignedLegionName = message.LegionName;
            Data.AssignedLegionNumber = message.LegionNumber;

            await _bus.Publish(new RemembrancerReportMessage
            {
                Origin = Shared.Routing.Buses.AdeptusAdministratum,
                Message = $"The {Data.AssignedLegionName} have been assigned to the compliance operation on {Data.WorldName}."
            });

            await _bus.Send(new InitiateReconMessage
            {
                ComplianceOrderID = Data.ComplianceOrderID
            });
        }

        public async Task Handle(ReconInitiatedMessage message)
        {
            await _bus.Publish(new RemembrancerReportMessage
            {
                Origin = Shared.Routing.Buses.AdeptusAdministratum,
                Message = $"The {Data.AssignedLegionName} have begun recon on {Data.WorldName}."
            });
        }

        public async Task Handle(ReconCompletedMessage message)
        {
            Data.RecommendedImperialGuardCompanies = message.RecommendedImperialGuardCompanies;
            Data.RecommendedTanks = message.RecommendedTanks;

            await _bus.Publish(new RemembrancerReportMessage
            {
                Origin = Shared.Routing.Buses.AdeptusAdministratum,
                Message = $"The {Data.AssignedLegionName} have completed recon on {Data.WorldName}. Recommendations: {message.RecommendedImperialGuardCompanies} guard companies, {message.RecommendedTanks} tanks."
            });

            await _bus.Send(new CreateImperialGuardDeploymentMessage
            {
                ComplianceOrderID = message.ComplianceOrderID,
                GuardCompanies = message.RecommendedImperialGuardCompanies,
                Tanks = message.RecommendedTanks,
                WorldName = Data.WorldName
            });
        }

        public async Task Handle(ImperialGuardDeploymentCreatedMessage message)
        {
            await _bus.Publish(new RemembrancerReportMessage
            {
                Origin = Shared.Routing.Buses.AdeptusAdministratum,
                Message = $"An imperial guard deployment has been created and is in transit to {Data.WorldName}."
            });
        }

        public async Task Handle(ImperialGuardDeploymentArrivedMessage message)
        {
            await _bus.Publish(new RemembrancerReportMessage
            {
                Origin = Shared.Routing.Buses.AdeptusAdministratum,
                Message = $"An imperial guard deployment has arrived on {Data.WorldName}."
            });
        }

        public async Task Handle(ImperialGuardDeploymentEradicatedMessage message)
        {
            await _bus.Publish(new RemembrancerReportMessage
            {
                Origin = Shared.Routing.Buses.AdeptusAdministratum,
                Message = $"An imperial guard deployment has been eradicated on {Data.WorldName}. Ordering reinforcements."
            });

            await _bus.Send(new CreateImperialGuardReinforcementMessage
            {
                ComplianceOrderID = message.ComplianceOrderID,
                GuardCompanies = Data.RecommendedImperialGuardCompanies,
                Tanks = Data.RecommendedTanks,
                WorldName = Data.WorldName
            });
        }

        public async Task Handle(ImperialGuardReinforcementCreatedMessage message)
        {
            await _bus.Publish(new RemembrancerReportMessage
            {
                Origin = Shared.Routing.Buses.AdeptusAdministratum,
                Message = $"An imperial guard reinforcement deployment has been created and is in transit to {Data.WorldName}."
            });
        }

        public async Task Handle(ImperialGuardReinforcementArrivedMessage message)
        {
            await _bus.Publish(new RemembrancerReportMessage
            {
                Origin = Shared.Routing.Buses.AdeptusAdministratum,
                Message = $"An imperial guard reinforcement deployment has arrived on {Data.WorldName}."
            });
        }

        public async Task Handle(EnemyForcesEradicatedMessage message)
        {
            await _bus.Publish(new RemembrancerReportMessage
            {
                Origin = Shared.Routing.Buses.AdeptusAdministratum,
                Message = $"Enemy forces have been eliminated on {Data.WorldName} in the Name Of The Emperor, beloved by all."
            });

            await _bus.Publish(new RemembrancerReportMessage
            {
                Origin = Shared.Routing.Buses.AdeptusAdministratum,
                Message = $"By the power vested in me by the High Lords of Terra, I declare that world of {Data.WorldName} has been brought to Imperial Compliance, and has been rightfully returned to the Imperium of man. The Emperor protects."
            });

            MarkAsComplete();
        }
    }

    public class ComplianceSagaData : SagaData
    {
        public Guid ComplianceOrderID { get; set; }
        public string WorldName { get; set; }
        public string AssignedLegionName { get; set; }
        public int AssignedLegionNumber { get; set; }
        public int RecommendedTanks { get; set; }
        public int RecommendedImperialGuardCompanies { get; set; }
    }
}
