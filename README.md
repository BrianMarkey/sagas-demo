# sagas-demo

This demonstration is meant to show a way of implementing messaging sagas in dotnet core using [Rebus](https://github.com/rebus-org), using either SQL Server or RabbitMQ.

Use the rebuild-rabbit.ps1 script to run a Docker container to act as the transport.

Use the rebuild-sql.ps1 script to run a Docker container to act as the transport.

The choreography directory contains an example of how a messaging saga using [chroreography](https://microservices.io/patterns/data/saga.html#example-choreography-based-saga) and the orchestration folder contains an [orchestration](https://microservices.io/patterns/data/saga.html#example-orchestration-based-saga) example.

To run one of the demos:
- To use a RabbitMQ transport run the rebuild-rabbit.ps1, for SQL Server run rebuild-sql.ps1. These scripts will start a Docker container running sql or rabbitmq.
- Choose a demo type (choreography or orchestration)
- Run all of of the dotnet core applications for a demo type (AdeptusAdministratum, AdeptusAstartes, ImperialGuard, Inquisition). To use a SQL Server transport just start the app with `dotnet run`. For RabbitMQ `dotnet run -- "transport=rabbitmq"`
- Navigate to http://localhost:5000 to bring up the Inquisition page to monitor the messages.
- Navigate to http://localhost:5002 to bring up the Adeptus Administratum page.
- Select a world and click Submit to start the compliance saga.
