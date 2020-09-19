docker stop sql
docker rm sql
docker run -d -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=umbasa11!" -p 11433:1433 --name sql -d mcr.microsoft.com/mssql/server:2019-latest
Start-Sleep -s 30
docker exec sql /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P umbasa11! -d master -q "CREATE DATABASE Rebus"
