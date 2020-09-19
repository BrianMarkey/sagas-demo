docker stop rabbit
docker rm rabbit
docker run -d --hostname rebus-rabbit --name rabbit -p 5672:5672 -p 8080:15672 rabbitmq:3-management
