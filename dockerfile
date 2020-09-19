FROM mcr.microsoft.com/mssql/server:2019-latest
COPY ./create-db.sql .
COPY ./db-initialization.sh .
ENV ACCEPT_EULA Y
ENV sa_password umbasa11!
CMD CMD /bin/bash ./db-initialization.sh & /opt/mssql/bin/sqlservr
